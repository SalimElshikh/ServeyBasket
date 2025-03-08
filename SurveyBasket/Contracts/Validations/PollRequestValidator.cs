namespace SurveyBasket.Contracts.Validations;

public class PollRequestValidator : AbstractValidator<PollRequest>
{
    public PollRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.Description)
            .NotEmpty()
            .Length(3, 200);

    }
}
