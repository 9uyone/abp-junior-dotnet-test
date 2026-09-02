using ABP_test_task.DTOs.Halls;
using ABP_test_task.Services.Hall;
using ABP_test_task.Validators.Halls;
using FluentValidation;

namespace ABP_test_task.Endpoints;

public static class HallEndpoints {
	public static RouteGroupBuilder MapHallEndpoints(this RouteGroupBuilder group) {
		group.MapGet("/", GetAllHallsAsync);
		group.MapPost("/", CreateHallAsync);
		group.MapPut("/{id:int}", UpdateHallAsync);
		group.MapDelete("/{id:int}", DeleteHallAsync);
		group.MapGet("/available", GetAvailableHallsAsync);

		return group;
	}

	private static async Task<IResult> GetAllHallsAsync(IHallService service, CancellationToken cancellationToken) {
		var halls = await service.GetAllAsync(cancellationToken);
		return TypedResults.Ok(halls);
	}

	private static async Task<IResult> CreateHallAsync(CreateHallRequest request, IValidator<CreateHallRequest> validator, IHallService service, CancellationToken cancellationToken) {
		await validator.ValidateAndThrowAsync(request, cancellationToken);
		var id = await service.CreateHallAsync(request, cancellationToken);
		return TypedResults.Created($"/api/halls/{id}", id);
	}

	private static async Task<IResult> UpdateHallAsync(int id, UpdateHallRequest request, IValidator<UpdateHallRequest> validator, IHallService service, CancellationToken cancellationToken) {
		await validator.ValidateAndThrowAsync(request, cancellationToken);
		var updated = await service.UpdateHallAsync(id, request, cancellationToken);
		return updated ? TypedResults.NoContent() : TypedResults.NotFound();
	}

	private static async Task<IResult> DeleteHallAsync(int id, IHallService service, CancellationToken cancellationToken) {
		var deleted = await service.DeleteHallAsync(id, cancellationToken);
		return deleted ? TypedResults.NoContent() : TypedResults.NotFound();
	}

	private static async Task<IResult> GetAvailableHallsAsync(DateOnly date, TimeOnly startTime, int durationHours, IValidator<FindAvailableHallsQuery> validator, IHallService service, CancellationToken cancellationToken) {
		var query = new FindAvailableHallsQuery(date, startTime, durationHours);
		await validator.ValidateAndThrowAsync(query, cancellationToken);
		var halls = await service.FindAvailableHallsAsync(query, cancellationToken);
		return TypedResults.Ok(halls);
	}
}
