using Hangfire;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Contracts.Register;
using SurveyBasket.Errors;
using SurveyBasket.Helpers;
using System.Security.Cryptography;

namespace SurveyBasket.Services;

public class AuthService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly int _refreshtokenExpiryDays = 14;


    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        // Check User 

        var user = await _userManager.FindByEmailAsync(email);

        if(user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        //if(await _userManager.FindByEmailAsync(email) is not { } user)
        //    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if(result.Succeeded)
        {
            //Generate Jwt Token 
            var (token, expiresIn) = _jwtProvider.GeneratedToken(user);
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
        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);

        
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

    public async Task<Result> RegistraterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var emailIsExists = await _userManager.Users.AnyAsync(x=>x.Email == request.Email , cancellationToken);
        if (emailIsExists)
            return Result.Failure(UserErrors.DublicatedEmail);

        var user = request.Adapt<ApplicationUser>();
        user.UserName = request.UserName;
        var result = await _userManager.CreateAsync(user,request.Password);

        var error = result.Errors.FirstOrDefault(); 

        if(result.Succeeded)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            _logger.LogInformation("Confirmation Code:  {code}",code);

            //TODO : Send Email 
            await SendConfirmationEmail(user,code);
            return Result.Success();
        }
        return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request, CancellationToken cancellationToken = default)
    {
        if (await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.HasConfirmedEmail);

        var code = request.Code;
        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        }
        catch (FormatException)
        {
            return Result.Failure(UserErrors.InvalidCode);
        }

        var result = await _userManager.ConfirmEmailAsync(user, code);


        if (result.Succeeded)
            return Result.Success();


        var error = result.Errors.FirstOrDefault();
        return Result.Failure(new Error(error!.Code, error.Description, StatusCodes.Status400BadRequest));

    }
    public async Task<Result> ResentConfirmationEmail(ResendConfirmEmailRequest request)
    {
        if (await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.HasConfirmedEmail);

        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        _logger.LogInformation("Confirmation code : {code}", code);
        await   SendConfirmationEmail(user, code);
        return Result.Success();
    }
    public async Task<Result> SendResetPasswordCodeAsync(string email)
    {
        
        if (await _userManager.FindByEmailAsync(email) is not { } user)
            return Result.Success();

        if (!user.EmailConfirmed)
            return Result.Failure(UserErrors.EmailNotConfirmed);


        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        _logger.LogInformation("Reset Code : {code}", code);
        await SendResetPasswordEmail(user, code);
        return Result.Success();
    }
    private async Task SendResetPasswordEmail(ApplicationUser user, string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        var emailBody = EmailBodyBuilder.GenerateEmailBody("ForgetPassword",
            new Dictionary<string, string>
            {
                    {"{{name}}",user.FirstName },
                    {"{{action_url",$"{origin}/auth/forgetpassword?userId={user.Email}&code={code}" }
            });


        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Reset Password", emailBody));
        await Task.CompletedTask;

    }
    public async Task<Result> ResetPassowrdRequest(ResetPasswordRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null || !user.EmailConfirmed)
            return Result.Failure(UserErrors.InvalidCode);

        IdentityResult result; 
        try
        {
            var code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
            result = await _userManager.ResetPasswordAsync(user , code , request.NewPassword);
        }
        catch (FormatException )
        {
            result = IdentityResult.Failed(_userManager.ErrorDescriber.InvalidToken());
        }
        if (result.Succeeded)
            return Result.Success();
        var error = result.Errors.First();
        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));

    }
    private static string GenerateFefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    private async Task SendConfirmationEmail(ApplicationUser user , string code)
    {
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            new Dictionary<string, string>
            {
                    {"{{name}}",user.FirstName },
                    {"{{action_url",$"{origin}/auth/emaiConfirmation?userId={user.Id}&code={code}" }
            });


        BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confimation", emailBody));
        await Task.CompletedTask;

    }

}
