using SurveyBasket.Abstractions.Const;

namespace SurveyBasket.Contracts.Users;

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty();
            
        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .Matches(RegexPatterns.Password)
            .WithMessage("Password Should Be at least 8 digit ans should contain LowerCase , nonalphapitic and UpperCAse")
            .NotEqual(x=>x.CurrentPassword)
            .WithMessage("New Password could not be same as the current password ");
    }
}
    
