namespace ABP_test_task.Services.Booking;

public class BookingTimePolicy : IBookingTimePolicy {
	public TimeOnly WorkingDayStart { get; } = new(9, 0);
	public TimeOnly WorkingDayEnd { get; } = new(23, 0);

	public void EnsureWithinWorkingHours(DateOnly date, TimeOnly startTime, int durationHours) {
		if (durationHours <= 0)
			throw new ArgumentException("Duration must be greater than zero.");

		var start = ToUtcDateTime(date, startTime);
		var end = start.AddHours(durationHours);
		var workingStart = ToUtcDateTime(date, WorkingDayStart);
		var workingEnd = ToUtcDateTime(date, WorkingDayEnd);

		if (start < workingStart || end > workingEnd)
			throw new ArgumentException("Requested time must be within working hours from 09:00 to 23:00.");
	}

	public DateTime ToUtcDateTime(DateOnly date, TimeOnly time) {
		return DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Utc);
	}

	public bool HasOverlap(DateTime leftStart, int leftDurationHours, DateTime rightStart, int rightDurationHours) {
		var leftEnd = leftStart.AddHours(leftDurationHours);
		var rightEnd = rightStart.AddHours(rightDurationHours);
		return leftStart < rightEnd && rightStart < leftEnd;
	}
}
