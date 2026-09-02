namespace ABP_test_task.DTOs.Bookings;

public record BookingConfirmationDto(
	int BookingId,
	int HallId,
	string HallName,
	DateTime StartTime,
	int DurationHours,
	IReadOnlyList<BookingServiceDto> Services,
	decimal TotalPrice
);

public record BookingServiceDto(
	int ServiceId,
	string ServiceName,
	decimal ServicePrice
);
