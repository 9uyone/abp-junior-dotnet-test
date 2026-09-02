using ABP_test_task.DTOs.Halls;
using FluentValidation;

namespace ABP_test_task.Validators.Halls;

public class FindAvailableHallsQueryValidator : AbstractValidator<FindAvailableHallsQuery> {
	public FindAvailableHallsQueryValidator() {
		RuleFor(x => x.Date)
			.NotEmpty();

		RuleFor(x => x.StartTime)
			.NotEmpty();

		RuleFor(x => x.DurationHours)
			.GreaterThan(0);
	}
}
