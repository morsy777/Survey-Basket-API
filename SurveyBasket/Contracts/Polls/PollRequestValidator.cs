using FluentValidation;

namespace SurveyBasket.Contracts.Polls;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
	public PollRequestValidator()
	{
		RuleFor(x => x.Title)
			.NotEmpty()
			.Length(3, 100);

		RuleFor(x => x.Summary)
			.NotEmpty()
			.Length(3, 1500);

		RuleFor(x => x.StartsAt)
			.NotEmpty()
			.GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));

		RuleFor(x => x.EndsAt)
			.NotEmpty();

		RuleFor(x => x)
			.Must(HasValidDates)
			.WithName(nameof(PollRequest.EndsAt))
			.WithMessage("{PropertyName} must be greater than or equal Start Date.");

    }

	private bool HasValidDates(PollRequest Pollrequest) => 
		Pollrequest.EndsAt >= Pollrequest.StartsAt;
	
}
