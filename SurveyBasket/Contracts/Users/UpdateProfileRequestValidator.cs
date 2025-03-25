﻿namespace SurveyBasket.Contracts.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .Length(3,50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .Length(3,50);
        
    }
}
