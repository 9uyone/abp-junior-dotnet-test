using ABP_test_task.Data;
using ABP_test_task.DTOs.Halls;
using ABP_test_task.Entities;
using ABP_test_task.Services.Booking;
using Microsoft.EntityFrameworkCore;
using HallServiceEntity = ABP_test_task.Entities.HallService;

namespace ABP_test_task.Services.Hall;

public class HallService(AppDbContext context, IBookingTimePolicy bookingTimePolicy) : IHallService {
	public async Task<IReadOnlyList<AvailableHallDto>> GetAllAsync(CancellationToken cancellationToken) {
		return await context.Halls
			.AsNoTracking()
			.Select(h => new AvailableHallDto(
				h.Id,
				h.Name,
				h.Capacity,
				h.BasePricePerHour,
				h.HallServices.Select(hs => new HallServiceDto(
					hs.ServiceId,
					hs.Service.Name,
					hs.Price
				))
			))
			.ToListAsync(cancellationToken);
	}

	public async Task<int> CreateHallAsync(CreateHallRequest request, CancellationToken cancellationToken) {
		var serviceIds = request.Services.Select(service => service.ServiceId).ToArray();
		await EnsureServicesExistAsync(serviceIds, cancellationToken);

		var hall = new ConferenceHall {
			Name = request.Name,
			Capacity = request.Capacity,
			BasePricePerHour = request.BasePricePerHour,
			HallServices = request.Services.Select(service => new HallServiceEntity {
				ServiceId = service.ServiceId,
				Price = service.Price
			}).ToList()
		};

		context.Halls.Add(hall);
		await context.SaveChangesAsync(cancellationToken);
		return hall.Id;
	}

	public async Task<bool> UpdateHallAsync(int id, UpdateHallRequest request, CancellationToken cancellationToken) {
		var hall = await context.Halls
			.AsNoTracking()
			.FirstOrDefaultAsync(h => h.Id == id, cancellationToken);

		if (hall is null)
			return false;

		await EnsureServicesExistAsync(request.Services.Select(service => service.ServiceId).ToArray(), cancellationToken);

		hall.Name = request.Name;
		hall.Capacity = request.Capacity;
		hall.BasePricePerHour = request.BasePricePerHour;

		var incomingServiceIds = request.Services.Select(s => s.ServiceId).ToHashSet();
		var existingHallServices = await context.HallServices
			.Where(hs => hs.HallId == id)
			.ToListAsync(cancellationToken);

		var existingServiceIds = existingHallServices.ToDictionary(hs => hs.ServiceId);

		foreach (var service in request.Services) {
			if (existingServiceIds.TryGetValue(service.ServiceId, out var hallService)) {
				hallService.Price = service.Price;
				context.HallServices.Update(hallService);
				continue;
			}

			context.HallServices.Add(new HallServiceEntity {
				HallId = id,
				ServiceId = service.ServiceId,
				Price = service.Price
			});
		}

		foreach (var hallService in existingHallServices.Where(hs => !incomingServiceIds.Contains(hs.ServiceId)))
			context.HallServices.Remove(hallService);

		await context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<bool> DeleteHallAsync(int id, CancellationToken cancellationToken) {
		var hall = await context.Halls.FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
		if (hall is null)
			return false;

		var now = DateTime.UtcNow;

		if (await context.Bookings
			.AsNoTracking()
			.AnyAsync(booking => booking.HallId == id && booking.EndTime > now, cancellationToken))
			throw new InvalidOperationException("Hall cannot be deleted while active or future bookings exist.");

		context.Halls.Remove(hall);
		await context.SaveChangesAsync(cancellationToken);
		return true;
	}

	public async Task<IReadOnlyList<AvailableHallDto>> FindAvailableHallsAsync(FindAvailableHallsQuery query, CancellationToken cancellationToken) {
		bookingTimePolicy.EnsureWithinWorkingHours(query.Date, query.StartTime, query.DurationHours);

		var start = bookingTimePolicy.ToUtcDateTime(query.Date, query.StartTime);
		var end = start.AddHours(query.DurationHours);

		return await context.Halls
			.AsNoTracking()
			.Where(hall => !hall.Bookings.Any(booking => booking.StartTime < end && booking.EndTime > start))
			.Select(hall => new AvailableHallDto(
				hall.Id,
				hall.Name,
				hall.Capacity,
				hall.BasePricePerHour,
				hall.HallServices.Select(service => new HallServiceDto(
					service.ServiceId,
					service.Service.Name,
					service.Price
				))
			))
			.ToListAsync(cancellationToken);
	}

	private async Task EnsureServicesExistAsync(int[] serviceIds, CancellationToken cancellationToken) {
		if (serviceIds.Length == 0)
			return;

		var existingServiceIds = await context.Services
			.AsNoTracking()
			.Where(service => serviceIds.Contains(service.Id))
			.Select(service => service.Id)
			.ToListAsync(cancellationToken);

		var missingServiceIds = serviceIds.Except(existingServiceIds).ToArray();
		if (missingServiceIds.Length > 0)
			throw new ArgumentException($"Unknown service ids: {string.Join(", ", missingServiceIds)}");
	}
}
