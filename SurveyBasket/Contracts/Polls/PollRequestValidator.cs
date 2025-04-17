using FluentValidation;

namespace SurveyBasket.Contracts.Polls;

public class AuthRequestValidator : AbstractValidator<PollRequest>
{
    private readonly ApplicationDbContext context;

    public AuthRequestValidator(ApplicationDbContext context)
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .Length(3, 50)
            .MustAsync(async (title, cancellation) =>
             {
                 return !await context.Polls.AnyAsync(p => p.Title == title, cancellation);
             }).WithMessage("العنوان المدخل موجود بالفعل، يرجى اختيار عنوان آخر.");

        RuleFor(x => x.Sammary)
            .NotEmpty()
            .Length(3, 1500);

        RuleFor(x => x.StartAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.Now));

        RuleFor(x => x.EndAt)
            .NotEmpty()
            .GreaterThanOrEqualTo(x => x.StartAt);
        this.context = context;


        //RuleFor(x => x)
        //    .Must(HasValidDate)
        //    .WithName(nameof(PollRequest.EndAt))
        //    .WithMessage("{PropertyName} must be greater or equal than Strat At Date");


    }
    //private bool HasValidDate(PollRequest request)
    //{
    //    return request.EndAt >= request.StartAt;
    //}

}
