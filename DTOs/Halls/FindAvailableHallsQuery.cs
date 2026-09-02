namespace ABP_test_task.DTOs.Halls;

public record FindAvailableHallsQuery(
	DateOnly Date,
	TimeOnly StartTime,
	int DurationHours
);
