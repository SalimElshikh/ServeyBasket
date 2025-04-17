using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.Extentions;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class ResultsController(IResultService resultService) : ControllerBase
{
    private readonly IResultService _resultService = resultService;

    [HttpGet("data-row")]
    public async Task<IActionResult> GetPollVotes(int pollId , CancellationToken cancellationToken = default)
    {
        var result = await _resultService.GetPollVotesAsync(pollId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }


    [HttpGet("votes-per-day")]
    public async Task<IActionResult> VotesPerDay([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _resultService.GetPollVotesPerDayAsync(pollId, cancellationToken);
        //TODO : Retutn Problem 

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpGet("votes-per-question")]
    public async Task<IActionResult> VotesPerQuestion([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = User.GetUserId();

        var result = await _resultService.GetVotePerQuestionAsync(pollId, cancellationToken);
        //TODO : Retutn Problem 

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }



}
