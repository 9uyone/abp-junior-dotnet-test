namespace ABP_test_task.Services.Pricing;

public class RentalPriceCalculator : IRentalPriceCalculator {
	public decimal CalculateTotal(decimal basePricePerHour, DateTime startTime, int durationHours, IEnumerable<decimal> servicePrices) {
		decimal hallTotal = 0m;

		for (int i = 0; i < durationHours; i++) {
			var currentHour = startTime.AddHours(i).Hour;
			hallTotal += GetMultiplierForHour(currentHour) * basePricePerHour;
		}

		return hallTotal + servicePrices.Sum();
	}

	private decimal GetMultiplierForHour(int hour) => hour switch {
		>= 6 and < 9 => 0.9m,   // 10% знижки
		>= 12 and < 14 => 1.15m,  // 15% націнки
		>= 18 and < 23 => 0.8m,   // 20% знижки
		_ => 1.0m    // стандартний тариф для всіх інших годин
	};
}
