namespace ABP_test_task.DTOs.Halls;

public record UpdateHallRequest(
	string Name,
	int Capacity,
	decimal BasePricePerHour,
	IReadOnlyCollection<HallServiceRequestDto> Services
);
