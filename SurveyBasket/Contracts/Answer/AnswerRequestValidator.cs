namespace SurveyBasket.Contracts.Answer;

public class AnswerRequestValidator : AbstractValidator<AnswerRequest>
{
    public AnswerRequestValidator()
    {
        RuleFor(x => x.Content)
            .NotEmpty()
            .Length(3, 1000);

        //RuleFor(x => x.Answers)
        //    .Must(x => x.Count > 1)
        //    .WithMessage("Question Should Has At Least 2 Answer ");


        //RuleFor(x => x.Answers)
        //    .Must(x => x.Distinct().Count() == x.Count )
        //    .WithMessage("You Cannot Add Duplicated Answer For The Same Question ");


    }
}
