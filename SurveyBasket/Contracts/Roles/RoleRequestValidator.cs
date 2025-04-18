﻿namespace SurveyBasket.Contracts.Roles;

public class RoleRequestValidator : AbstractValidator<RoleRequest>
{
    public RoleRequestValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty()
            .Length(3,200); 


        RuleFor(x => x.Permissions)
            .NotNull()
            .Empty();

        RuleFor(x => x.Permissions)
            .Must(x => x.Distinct().Count() == x.Count)
            .When(x => x.Permissions != null);

    }
}
