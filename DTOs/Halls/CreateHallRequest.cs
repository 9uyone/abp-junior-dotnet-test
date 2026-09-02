namespace ABP_test_task.DTOs.Halls;

public record CreateHallRequest(
	string Name,
	int Capacity,
	decimal BasePricePerHour,
	IReadOnlyCollection<HallServiceRequestDto> Services = default!
);
