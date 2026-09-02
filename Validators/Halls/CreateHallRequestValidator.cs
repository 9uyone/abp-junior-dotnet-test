using ABP_test_task.DTOs.Halls;
using FluentValidation;

namespace ABP_test_task.Validators.Halls;

public class CreateHallRequestValidator : AbstractValidator<CreateHallRequest> {
	public CreateHallRequestValidator() {
		RuleFor(x => x.Name)
			.NotEmpty()
			.MaximumLength(100);

		RuleFor(x => x.Capacity)
			.GreaterThan(0);

		RuleFor(x => x.BasePricePerHour)
			.GreaterThan(0);

		RuleFor(x => x.Services)
			.NotNull();

		RuleForEach(x => x.Services)
			.SetValidator(new HallServiceRequestDtoValidator());

		RuleFor(x => x.Services)
			.Must(services => services == null || services.Select(s => s.ServiceId).Distinct().Count() == services.Count)
			.WithMessage("Services must not contain duplicate ServiceId values.");
	}
}

internal sealed class HallServiceRequestDtoValidator : AbstractValidator<HallServiceRequestDto> {
	public HallServiceRequestDtoValidator() {
		RuleFor(x => x.ServiceId)
			.GreaterThan(0);

		RuleFor(x => x.Price)
			.GreaterThan(0);
	}
}
