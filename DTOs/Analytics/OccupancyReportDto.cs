namespace ABP_test_task.DTOs.Analytics;

public record HallOccupancyDto(
	int HallId,
	string HallName,
	int BookedHours,
	double OccupancyPercentage
);

public record OccupancyReportDto(
	DateTime From,
	DateTime To,
	int TotalPossibleHoursPerHall,
	IReadOnlyList<HallOccupancyDto> Halls,
	string? MostPopularHallName
);
