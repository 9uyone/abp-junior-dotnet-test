namespace ABP_test_task.DTOs.Analytics;

public record HallRevenueDto(
	int HallId,
	string HallName,
	int TotalBookings,
	decimal Revenue
);

public record RevenueReportDto(
	DateOnly From,
	DateOnly To,
	decimal TotalRevenue,
	IReadOnlyList<HallRevenueDto> Halls
);
