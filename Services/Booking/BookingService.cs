using ABP_test_task.Data;
using ABP_test_task.DTOs.Bookings;
using ABP_test_task.Services.Pricing;
using Microsoft.EntityFrameworkCore;
using BookingServiceEntity = ABP_test_task.Entities.BookingService;

namespace ABP_test_task.Services.Booking;

public class BookingService(AppDbContext context, IBookingTimePolicy bookingTimePolicy, IRentalPriceCalculator priceCalculator) : IBookingService {
	public async Task<BookingConfirmationDto> CreateBookingAsync(CreateBookingRequest request, CancellationToken cancellationToken) {
		bookingTimePolicy.EnsureWithinWorkingHours(request.Date, request.StartTime, request.DurationHours);

		var hall = await context.Halls
			.AsNoTracking()
			.FirstOrDefaultAsync(h => h.Id == request.HallId, cancellationToken);

		if (hall is null)
			throw new ArgumentException($"Hall with ID {request.HallId} not found.");

		var startDateTime = bookingTimePolicy.ToUtcDateTime(request.Date, request.StartTime);
		var endDateTime = startDateTime.AddHours(request.DurationHours);

		var hasConflict = await context.Bookings
			.Where(b => b.HallId == request.HallId &&
					b.StartTime < endDateTime &&
					b.EndTime > startDateTime)
			.AnyAsync(cancellationToken);

		if (hasConflict)
			throw new InvalidOperationException("The requested time slot has a conflict with an existing booking.");

		var hallServices = await context.HallServices
			.Where(hs => hs.HallId == request.HallId && request.ServiceIds.Contains(hs.ServiceId))
			.ToListAsync(cancellationToken);

		if (hallServices.Count != request.ServiceIds.Count)
			throw new ArgumentException("One or more requested services are not available for this hall.");

		var servicePrices = hallServices.Select(hs => hs.Price).ToList();
		var totalPrice = priceCalculator.CalculateTotal(hall.BasePricePerHour, startDateTime, request.DurationHours, servicePrices);

		var booking = new Entities.Booking {
			HallId = request.HallId,
			StartTime = startDateTime,
			DurationHours = request.DurationHours,
			TotalPrice = totalPrice,
			BookingServices = hallServices.Select(hs => new BookingServiceEntity {
				ServiceId = hs.ServiceId,
				PriceAtBooking = hs.Price
			}).ToList()
		};

		context.Bookings.Add(booking);
		await context.SaveChangesAsync(cancellationToken);

		return new BookingConfirmationDto(
			booking.Id,
			hall.Id,
			hall.Name,
			booking.StartTime,
			booking.DurationHours,
			hallServices.Select(hs => new BookingServiceDto(hs.ServiceId, hs.Service.Name, hs.Price)).ToList(),
			booking.TotalPrice
		);
	}
}
