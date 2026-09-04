namespace ABP_test_task.Services.Booking;

public class BookingTimePolicy : IBookingTimePolicy {
	public TimeOnly WorkingDayStart { get; } = new(6, 0); // або 9, 0 згідно з ТЗ
	public TimeOnly WorkingDayEnd { get; } = new(23, 0);

	public void EnsureWithinWorkingHours(DateOnly date, TimeOnly startTime, int durationHours) {
		if (durationHours <= 0)
			throw new ArgumentException("Duration must be greater than zero.");

		// Перевіряємо вихід за межі доби або закінчення після закриття
		// Якщо startTime + durationHours перетинає північ, AddHours поверне менший час
		var endTime = startTime.AddHours(durationHours, out int wrappedDays);

		if (wrappedDays > 0 || startTime < WorkingDayStart || endTime > WorkingDayEnd)
			throw new ArgumentException($"Requested time must be within working hours from {WorkingDayStart:HH\\:mm} to {WorkingDayEnd:HH\\:mm}.");
	}

	public DateTime ToUtcDateTime(DateOnly date, TimeOnly time) =>
		DateTime.SpecifyKind(date.ToDateTime(time), DateTimeKind.Utc);
}
