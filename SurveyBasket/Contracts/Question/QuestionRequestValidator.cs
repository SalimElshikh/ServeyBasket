namespace SurveyBasket.Contracts.Question;

public class VoteAnswerRequestValidator : AbstractValidator<QuestionRequest>
{
    public VoteAnswerRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Length(3, 1000);

        RuleFor(x => x.Answers)
            .NotNull();

        RuleFor(x => x.Answers)
            .Must(x => x.Count > 1)
            .WithMessage("Question Should Has At Least 2 Answer ")
            .When(x => x.Answers != null);

        RuleFor(x => x.Answers)
            .Must(x => x.Distinct().Count() == x.Count )
            .WithMessage("You Cannot Add Duplicated Answer For The Same Question ")
            .When(x => x.Answers != null);



    }
}
