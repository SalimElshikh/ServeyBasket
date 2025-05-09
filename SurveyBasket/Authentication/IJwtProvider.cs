﻿namespace SurveyBasket.Authentication;

public interface IJwtProvider
{
    (string token, int expiresIn) GeneratedToken(ApplicationUser user);
    string? ValidateToken(string token);
}
