using ABP_test_task.DTOs.Bookings;
using ABP_test_task.Services.Booking;
using FluentValidation;

namespace ABP_test_task.Endpoints;

public static class BookingEndpoints {
	public static RouteGroupBuilder MapBookingEndpoints(this RouteGroupBuilder group) {
		group.MapPost("/", CreateBookingAsync)
			.Produces<BookingConfirmationDto>(201)
			.Produces(400)
			.Produces(409);

		return group;
	}

	private static async Task<IResult> CreateBookingAsync(CreateBookingRequest request, IValidator<CreateBookingRequest> validator, IBookingService service, CancellationToken cancellationToken) {
		await validator.ValidateAndThrowAsync(request, cancellationToken);

		var confirmation = await service.CreateBookingAsync(request, cancellationToken);
		return TypedResults.Created($"/api/bookings/{confirmation.BookingId}", confirmation);
	}
}
