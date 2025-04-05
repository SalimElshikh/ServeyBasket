using ApplicationLayer.Contracts.Users;
using ApplicationLayer.Services;
using ApplicationLayer.Abstractions;
using ApplicationLayer.Extentions;

namespace SurveyBasket.Controllers;
[Route("me")]
[ApiController]
[Authorize]
public class AccountsController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetPorfileAsync(User.GetUserId()!);
        return Ok(result.Value);
    }
    [HttpPut("info")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var result = await _userService.UpdateProfileAsync(User.GetUserId()!, request);
        return NoContent();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        ApplicationLayer.Abstractions.Result result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
