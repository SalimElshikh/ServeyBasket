namespace SurveyBasket.Authentication;

public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequest>
{

    public RefreshTokenValidator()
    {

        RuleFor(x => x.Token)
            .NotEmpty();
            


        RuleFor(x => x.RefreshToken)
            .NotEmpty();


    }


}
