﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Extentions;

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
        var result = await _userService.UpdateProfileAsync(User.GetUserId()!,request);
        return NoContent();
    }
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);
        return result.IsSuccess ? NoContent() : result.ToProblem();
    }
}
