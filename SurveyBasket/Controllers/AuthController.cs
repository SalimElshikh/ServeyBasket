using ApplicationLayer.Authentication;
using ApplicationLayer.Contracts.Authentication;
using ApplicationLayer.Contracts.Register;
using ApplicationLayer.Reposatories;
using ApplicationLayer.Abstractions;


namespace SurveyBasket.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    [HttpPost("")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest loginRequest, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(loginRequest.Email, loginRequest.Password, cancellationToken);

        return authResult.IsSuccess ? Ok(authResult.Value) : authResult.ToProblem();
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult is null ? BadRequest("Envlaid Token ") : Ok(authResult);
    }

    [HttpPost("revoke-refresh-token")]
    public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return result.IsSuccess ? Ok() : Problem(statusCode: StatusCodes.Status400BadRequest, title: result.Error.Code, detail: result.Error.Description);
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.RegistraterAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }

    [HttpPost("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _authService.ConfirmEmailAsync(request, cancellationToken);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("resend-confirm-email")]
    public async Task<IActionResult> ResendConfirmEmail([FromBody] ResendConfirmEmailRequest request)
    {
        var result = await _authService.ResentConfirmationEmail(request);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("forget-password")]
    public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequest request)
    {
        var result = await _authService.SendResetPasswordCodeAsync(request.Email);
        return result.IsSuccess ? Ok() : result.ToProblem();
    }
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPassowrdRequest(request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
