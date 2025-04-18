using Microsoft.AspNetCore.Authorization;
using SurveyBasket.Abstractions.Const;
using SurveyBasket.Contracts.Vote;
using SurveyBasket.Extentions;
using System.Security.Claims;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize(Roles = DefaultRoles.Member)]
public class VotesController(IQuestionService questionService, IVoteService voteService, IResultService resultService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteService _voteService = voteService;
    private readonly IResultService _resultService = resultService;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId , CancellationToken cancellationToken)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var result = await _questionService.GetAvailableAsync(pollId,userId!,cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> AddVote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _voteService.AddAsync(pollId, userId!,request, cancellationToken);
        return result.IsSuccess ? Created() : result.ToProblem();
    }
    
}
