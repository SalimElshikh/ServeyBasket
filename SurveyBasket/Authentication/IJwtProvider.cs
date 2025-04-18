namespace SurveyBasket.Authentication;

public interface IJwtProvider
{
    (string token, int expiresIn) GeneratedToken(ApplicationUser user,IEnumerable<string> roles,IEnumerable<string> permissions);
    string? ValidateToken(string token);
}
