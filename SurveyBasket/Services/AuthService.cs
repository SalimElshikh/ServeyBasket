
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using SurveyBasket.Abstractions;
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager, IJwtProvider jwtProvider) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly int _refreshtokenExpiryDays = 14;



    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // Check User 

        var user = await _userManager.FindByEmailAsync(email);
        if(user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        // Check Password

        var isValidPassword = await _userManager.CheckPasswordAsync(user, password);
        if (!isValidPassword)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);


        //Generate Jwt Token 
        var (token , expiresIn) = _jwtProvider.GeneratedToken(user);
        var refreshToken = GenerateFefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshtokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = refreshToken,
            ExpiresOn = refreshTokenExpiration,

        });
        await _userManager.UpdateAsync(user); 

        var response = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpiration);
        return Result.Success(response);
    }
    public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);
        if (userId is null)
            return null;
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) 
            return null;

        var userRefreshToken = user.RefreshTokens.SingleOrDefault( x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return null;

        userRefreshToken.RevokedOn = DateTime.UtcNow;
        // Generate New Token 
        var (NewToken, expiresIn) = _jwtProvider.GeneratedToken(user);
        var newRefreshToken = GenerateFefreshToken();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshtokenExpiryDays);

        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration,

        });
        await _userManager.UpdateAsync(user);
        return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, NewToken, expiresIn, newRefreshToken, refreshTokenExpiration);

    }
    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    private static string GenerateFefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
}
