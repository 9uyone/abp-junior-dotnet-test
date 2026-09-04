using ABP_test_task.Services.Pricing;
using FluentAssertions;

namespace Tests.Services;

public class RentalPriceCalculatorTests {
	private readonly RentalPriceCalculator _calculator = new();

	[Theory]
	[InlineData(10, 2, 2000, 4000)] // 10:00 - 12:00 (10, 11) -> 2 * 2000 * 1.0 = 4000
	[InlineData(15, 3, 1500, 4500)] // 15:00 - 18:00 (15, 16, 17) -> 3 * 1500 * 1.0 = 4500
	public void CalculateTotal_StandardHours_CalculatesWithoutDiscountsOrMarkups(
		int startHour,
		int durationHours,
		decimal basePrice,
		decimal expectedTotal)
	{
		var startTime = new DateTime(2026, 9, 1, startHour, 0, 0);
		var result = _calculator.CalculateTotal(basePrice, startTime, durationHours, []);
		result.Should().Be(expectedTotal);
	}

	[Theory]
	[InlineData(6, 3, 1000, 2700)] // 06:00 - 09:00 (6, 7, 8) -> 3 * 1000 * 0.9 = 2700
	[InlineData(8, 2, 2000, 3600)] // 08:00 - 10:00 (8, 9) -> 2 * 2000 * 0.9 = 3600
	public void CalculateTotal_MorningHours_AppliesTenPercentDiscount(
		int startHour,
		int durationHours,
		decimal basePrice,
		decimal expectedTotal)
	{
		var startTime = new DateTime(2026, 9, 1, startHour, 0, 0);
		var result = _calculator.CalculateTotal(basePrice, startTime, durationHours, []);
		result.Should().Be(expectedTotal);
	}

	[Theory]
	[InlineData(12, 2, 2000, 4600)] // 12:00 - 14:00 (12, 13) -> 2 * 2000 * 1.15 = 4600
	[InlineData(13, 2, 1000, 2300)] // 13:00 - 15:00 (13, 14) -> 2 * 1000 * 1.15 = 2300
	public void CalculateTotal_PeakHours_AppliesFifteenPercentMarkup(
		int startHour,
		int durationHours,
		decimal basePrice,
		decimal expectedTotal)
	{
		var startTime = new DateTime(2026, 9, 1, startHour, 0, 0);
		var result = _calculator.CalculateTotal(basePrice, startTime, durationHours, []);
		result.Should().Be(expectedTotal);
	}

	[Theory]
	[InlineData(19, 2, 2000, 3200)] // 19:00 - 21:00 (19, 20) -> 2 * 2000 * 0.8 = 3200
	[InlineData(21, 2, 1000, 1600)] // 21:00 - 23:00 (21, 22) -> 2 * 1000 * 0.8 = 1600
	public void CalculateTotal_EveningHours_AppliesTwentyPercentDiscount(
		int startHour,
		int durationHours,
		decimal basePrice,
		decimal expectedTotal)
	{
		var startTime = new DateTime(2026, 9, 1, startHour, 0, 0);
		var result = _calculator.CalculateTotal(basePrice, startTime, durationHours, []);
		result.Should().Be(expectedTotal);
	}

	[Fact]
	public void CalculateTotal_CrossingMultipleRateZonesWithServices_CalculatesCorrectSum() {
		// 11:00 до 16:00, базова ціна = 1000 грн/год
		// 11:00 - 12:00 -> 1000 * 1.0  = 1000 (стандарт)
		// 12:00 - 13:00 -> 1000 * 1.15 = 1150 (пік)
		// 13:00 - 14:00 -> 1000 * 1.15 = 1150 (пік)
		// 14:00 - 15:00 -> 1000 * 1.15 = 1150 (пік за поточною умовою <= 14)
		// 15:00 - 16:00 -> 1000 * 1.0  = 1000 (стандарт)
		// Разом оренда залу: 5450
		// Послуги: Проєктор (500) + Звук (700) + Wi-Fi (300) = 1500
		// Очікуваний тотал: 6950
		var startTime = new DateTime(2026, 9, 1, 11, 0, 0);
		const int durationHours = 5;
		const decimal basePricePerHour = 1000m;
		var servicePrices = new decimal[] { 500m, 700m, 300m };

		var result = _calculator.CalculateTotal(basePricePerHour, startTime, durationHours, servicePrices);
		result.Should().Be(6950m);
	}

	[Fact]
	public void CalculateTotal_ZeroDuration_ReturnsOnlyServicesSum() {
		var startTime = new DateTime(2026, 9, 1, 10, 0, 0);
		var servicePrices = new decimal[] { 500m, 300m };

		var result = _calculator.CalculateTotal(2000m, startTime, 0, servicePrices);
		result.Should().Be(800m);
	}
}