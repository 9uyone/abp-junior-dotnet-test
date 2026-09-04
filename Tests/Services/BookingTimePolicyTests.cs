using ABP_test_task.Services.Booking;
using FluentAssertions;

namespace Tests.Services;

public class BookingTimePolicyTests {
	private readonly BookingTimePolicy _policy = new();

	[Theory]
	[InlineData(6, 2)]   // 06:00 - 08:00 (рівно на старті)
	[InlineData(10, 4)]  // 10:00 - 14:00 (всередині дня)
	[InlineData(20, 3)]  // 20:00 - 23:00 (впритул до закриття)
	public void EnsureWithinWorkingHours_ValidInterval_DoesNotThrow(int hour, int duration) {
		// Arrange
		var date = new DateOnly(2026, 9, 1);
		var startTime = new TimeOnly(hour, 0);

		// Act
		var act = () => _policy.EnsureWithinWorkingHours(date, startTime, duration);

		// Assert
		act.Should().NotThrow();
	}

	[Theory]
	[InlineData(5, 2)]   // 05:00 - початок раніше за відкриття (06:00)
	[InlineData(22, 2)]  // 22:00 + 2 год = 00:00 (виходить за 23:00)
	[InlineData(23, 1)]  // 23:00 + 1 год = 00:00
	[InlineData(10, 0)]  // Тривалість 0 або менше
	[InlineData(10, -1)]
	public void EnsureWithinWorkingHours_InvalidInterval_ThrowsArgumentException(int hour, int duration) {
		// Arrange
		var date = new DateOnly(2026, 9, 1);
		var startTime = new TimeOnly(hour, 0);

		// Act
		var act = () => _policy.EnsureWithinWorkingHours(date, startTime, duration);

		// Assert
		act.Should().Throw<ArgumentException>();
	}
}