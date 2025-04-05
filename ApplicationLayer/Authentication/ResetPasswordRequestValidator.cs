using ApplicationLayer.Abstractions.Const;
using FluentValidation;

namespace ApplicationLayer.Authentication;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Code)
            .NotEmpty();

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should Be at least 8 digit ans should contain LowerCase , nonalphapitic and UpperCAse");

    }
}
