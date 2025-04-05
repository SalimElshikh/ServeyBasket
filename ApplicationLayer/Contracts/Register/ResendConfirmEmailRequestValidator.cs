using FluentValidation;

namespace ApplicationLayer.Contracts.Register;

public class ResendConfirmEmailRequestValidator : AbstractValidator<ResendConfirmEmailRequest>
{
    public ResendConfirmEmailRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
    }
}
