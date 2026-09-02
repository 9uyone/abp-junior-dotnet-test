namespace ABP_test_task.DTOs.Bookings;

public record CreateBookingRequest(
	int HallId,
	DateOnly Date,
	TimeOnly StartTime,
	int DurationHours,
	IReadOnlyCollection<int> ServiceIds
);
