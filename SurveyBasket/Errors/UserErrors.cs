﻿
namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials = new Error("User.Invalid Credentials", "Invalid Email Or Password ", StatusCodes.Status400BadRequest);
    public static readonly Error DublicatedEmail = new Error("User.DublicatedEmail ", "DublicatedEmail ", StatusCodes.Status409Conflict);
    public static readonly Error InvalidJwtToken = new Error("User.Invalid JwtToken", "Invalid Jwt Token  ", StatusCodes.Status400BadRequest);
    public static readonly Error InvalidRefreshToken = new Error("User.Invalid InvalidRefreshToken", "Invalid InvalidRefreshToken  ", StatusCodes.Status400BadRequest);
    public static readonly Error EmailNotConfirmed = new Error("User.NotConfirmedEmail", "Not Confirmed Email  ", StatusCodes.Status401Unauthorized);
    public static readonly Error InvalidCode = new Error("User.InvalidCode", "Invalid Code  ", StatusCodes.Status400BadRequest);
    public static readonly Error HasConfirmedEmail = new Error("User.HasConfirmedEmail", "HasConfirmedEmail  ", StatusCodes.Status400BadRequest);

}
