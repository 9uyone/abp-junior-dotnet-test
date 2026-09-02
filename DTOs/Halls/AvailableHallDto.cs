namespace ABP_test_task.DTOs.Halls;

public record AvailableHallDto(
	int Id,
	string Name,
	int Capacity,
	decimal BasePricePerHour,
	IEnumerable<HallServiceDto> Services
);
