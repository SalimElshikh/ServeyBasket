namespace SurveyBasket.Authentication;

public record    ResetPasswordRequest(
    string Email , 
    string Code,
    string NewPassword
);
