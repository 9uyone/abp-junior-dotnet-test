namespace ABP_test_task.Services.Pricing;

public interface IRentalPriceCalculator {
	decimal CalculateTotal(decimal basePricePerHours, DateTime startTime, int durationHours, IEnumerable<decimal> servicePrices);
}
