
namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials = new Error("User.Invalid Credentials", "Invalid Email Or Password ", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidJwtToken = new Error("User.Invalid JwtToken", "Invalid Jwt Token  ", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidRefreshToken = new Error("User.Invalid InvalidRefreshToken", "Invalid InvalidRefreshToken  ", StatusCodes.Status400BadRequest);

}
