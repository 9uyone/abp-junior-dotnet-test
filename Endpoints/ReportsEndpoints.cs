using ABP_test_task.DTOs.Analytics;
using ABP_test_task.Services.Analytics;

namespace ABP_test_task.Endpoints;

public static class ReportsEndpoints {
	public static IEndpointRouteBuilder MapReportsEndpoints(this IEndpointRouteBuilder app) {
		var group = app.MapGroup("/api/reports")
			.WithTags("Reports");

		group.MapGet("/revenue", GetRevenueAsync)
			.WithName("GetRevenueReport")
			.WithSummary("Get revenue report for a date range")
			.Produces<RevenueReportDto>(200)
			.Produces(400);

		group.MapGet("/occupancy", GetOccupancyAsync)
			.WithName("GetOccupancyReport")
			.WithSummary("Get occupancy report for a date range")
			.Produces<OccupancyReportDto>(200)
			.Produces(400);

		return app;
	}

	private static async Task<IResult> GetRevenueAsync(DateTime from, DateTime to, IAnalyticsService service, CancellationToken ct) {
		if (from > to)
			return Results.BadRequest("'from' date must be less than or equal to 'to' date.");

		var report = await service.GetRevenueReportAsync(from, to, ct);
		return TypedResults.Ok(report);
	}

	private static async Task<IResult> GetOccupancyAsync(DateTime from, DateTime to, IAnalyticsService service, CancellationToken ct) {
		if (from > to)
			return Results.BadRequest("'from' date must be less than or equal to 'to' date.");

		var report = await service.GetOccupancyReportAsync(from, to, ct);
		return TypedResults.Ok(report);
	}
}
