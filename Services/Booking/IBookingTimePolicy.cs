namespace ABP_test_task.Services.Booking;

public interface IBookingTimePolicy {
	TimeOnly WorkingDayStart { get; }
	TimeOnly WorkingDayEnd { get; }

	void EnsureWithinWorkingHours(DateOnly date, TimeOnly startTime, int durationHours);
	DateTime ToUtcDateTime(DateOnly date, TimeOnly time);
}
