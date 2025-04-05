using ApplicationLayer.Abstractions;
using ApplicationLayer.Authentication;
using ApplicationLayer.Contracts.Authentication;
using ApplicationLayer.Contracts.Register;

namespace ApplicationLayer.Reposatories;

public interface IAuthService
{
    Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default);
    Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default);
    Task<Result> RegistraterAsync(RegisterRequest request, CancellationToken cancellationToken = default);
    Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default);
    Task<Result> ResentConfirmationEmail(ResendConfirmEmailRequest request);
    Task<Result> SendResetPasswordCodeAsync(string email);
    Task<Result> ResetPassowrdRequest(ResetPasswordRequest request);
}
