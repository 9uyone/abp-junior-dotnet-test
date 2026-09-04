using ABP_test_task.DTOs.Analytics;

namespace ABP_test_task.Services.Analytics;

public interface IAnalyticsService {
	Task<RevenueReportDto> GetRevenueReportAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken);
	Task<OccupancyReportDto> GetOccupancyReportAsync(DateOnly from, DateOnly to, CancellationToken cancellationToken);
}
