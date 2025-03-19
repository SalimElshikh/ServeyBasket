﻿

namespace SurveyBasket.Reposatories;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email , string password , CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token , string refreshToken , CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token , string refreshToken, CancellationToken cancellationToken = default);

}
