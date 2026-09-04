using ABP_test_task.Data;
using ABP_test_task.DTOs.Analytics;
using ABP_test_task.Services.Booking;
using Microsoft.EntityFrameworkCore;

namespace ABP_test_task.Services.Analytics;

public class AnalyticsService(AppDbContext context, IBookingTimePolicy bookingTimePolicy) : IAnalyticsService {
	public async Task<RevenueReportDto> GetRevenueReportAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken) {
		// Convert DateOnly range to DateTime range (inclusive start, exclusive end)
		var start = from.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
		var endExclusive = to.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

		var bookings = await context.Bookings
			.AsNoTracking()
			.Where(b => b.StartTime >= start && b.StartTime < endExclusive)
			.Include(b => b.Hall)
			.GroupBy(b => new { b.HallId, b.Hall.Name })
			.Select(g => new HallRevenueDto(
				g.Key.HallId,
				g.Key.Name,
				g.Count(),
				g.Sum(b => b.TotalPrice)
			))
			.ToListAsync(cancellationToken);

		var totalRevenue = bookings.Sum(h => h.Revenue);

		return new RevenueReportDto(from, to, totalRevenue, bookings.AsReadOnly());
	}

	public async Task<OccupancyReportDto> GetOccupancyReportAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken) {
		if (to < from)
			throw new ArgumentException("'to' date must be greater than or equal to 'from' date.");

		var start = from.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
		var endExclusive = to.AddDays(1).ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);

		// .Hours або (int).TotalHours, а не .Days!
		int hoursPerDay = (int)(bookingTimePolicy.WorkingDayEnd - bookingTimePolicy.WorkingDayStart).TotalHours;
		var days = (endExclusive - start).Days;
		var totalPossibleHours = hoursPerDay * days;

		var hallOccupancy = await context.Halls
			.AsNoTracking()
			.Select(h => new {
				h.Id,
				h.Name,
				BookedHours = h.Bookings
					.Where(b => b.StartTime >= start && b.StartTime < endExclusive)
					.Sum(b => b.DurationHours)
			})
			.ToListAsync(cancellationToken);

		var occupancyData = hallOccupancy
			.Select(h => new HallOccupancyDto(
				h.Id,
				h.Name,
				h.BookedHours,
				totalPossibleHours > 0
					? Math.Round((h.BookedHours / (double)totalPossibleHours) * 100, 2)
					: 0
			))
			.OrderByDescending(h => h.BookedHours)
			.ToList();

		var mostPopularHall = occupancyData.FirstOrDefault()?.HallName;

		return new OccupancyReportDto(from, to, totalPossibleHours, occupancyData.AsReadOnly(), mostPopularHall);
	}
}
