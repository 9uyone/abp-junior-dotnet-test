using ABP_test_task.DTOs.Bookings;
using FluentValidation;

namespace ABP_test_task.Validators.Bookings;

public class CreateBookingRequestValidator : AbstractValidator<CreateBookingRequest> {
	public CreateBookingRequestValidator() {
		RuleFor(x => x.HallId)
			.GreaterThan(0)
			.WithMessage("Hall ID must be greater than zero.");

		RuleFor(x => x.Date)
			.NotEmpty()
			.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
			.WithMessage("Booking date must be today or in the future.");

		RuleFor(x => x.StartTime)
			.NotEmpty();

		RuleFor(x => x.DurationHours)
			.GreaterThan(0)
			.WithMessage("Duration must be greater than zero.");

		RuleFor(x => x.ServiceIds)
			.NotNull()
			.WithMessage("Service IDs collection must not be null.");
	}
}
