using ApplicationLayer.Abstractions.Const;
using FluentValidation;

namespace ApplicationLayer.Contracts.Register;

public class RegistrationRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegistrationRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should Be at least 8 digit ans should contain LowerCase , nonalphapitic and UpperCAse");

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3, 50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3, 50);
        RuleFor(x => x.UserName)
            .NotEmpty()
            .Length(3, 50);


    }
}
