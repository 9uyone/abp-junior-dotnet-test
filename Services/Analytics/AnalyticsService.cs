using ABP_test_task.Data;
using ABP_test_task.DTOs.Analytics;
using Microsoft.EntityFrameworkCore;

namespace ABP_test_task.Services.Analytics;

public class AnalyticsService(AppDbContext context) : IAnalyticsService {
	public async Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken) {
		var bookings = await context.Bookings
			.AsNoTracking()
			.Where(b => b.StartTime >= from && b.StartTime < to.AddDays(1))
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

	public async Task<OccupancyReportDto> GetOccupancyReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken) {
		const int hoursPerDay = 17; // 06:00 to 23:00
		var days = (to.Date - from.Date).Days + 1;
		var totalPossibleHours = hoursPerDay * days;

		var hallOccupancy = await context.Halls
			.AsNoTracking()
			.Select(h => new {
				h.Id,
				h.Name,
				BookedHours = h.Bookings
					.Where(b => b.StartTime >= from && b.StartTime < to.AddDays(1))
					.Sum(b => b.DurationHours)
			})
			.ToListAsync(cancellationToken);

		var occupancyData = hallOccupancy
			.Select(h => new HallOccupancyDto(
				h.Id,
				h.Name,
				h.BookedHours,
				h.BookedHours > 0 ? (h.BookedHours / (double)totalPossibleHours) * 100 : 0
			))
			.OrderByDescending(h => h.BookedHours)
			.ToList();

		var mostPopularHall = occupancyData.FirstOrDefault()?.HallName;

		return new OccupancyReportDto(from, to, totalPossibleHours, occupancyData.AsReadOnly(), mostPopularHall);
	}
}
