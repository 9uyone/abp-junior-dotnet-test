using ABP_test_task.DTOs.Analytics;

namespace ABP_test_task.Services.Analytics;

public interface IAnalyticsService {
	Task<RevenueReportDto> GetRevenueReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
	Task<OccupancyReportDto> GetOccupancyReportAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
}
