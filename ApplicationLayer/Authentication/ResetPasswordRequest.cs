namespace ApplicationLayer.Authentication;

public record    ResetPasswordRequest(
    string Email , 
    string Code,
    string NewPassword
);
