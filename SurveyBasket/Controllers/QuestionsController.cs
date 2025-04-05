using ApplicationLayer.Contracts.Question;
using ApplicationLayer.Reposatories;
using ApplicationLayer.Abstractions;

namespace SurveyBasket.Controllers;
[Route("api/polls/{pollId}/[controller]")]
[ApiController]
[Authorize]
public class QuestionsController(IQuestionService questionService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;

    [HttpGet("")]
    public async Task<IActionResult> GetAll([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAllAsync(pollId, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();

    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.GetAsync(pollId, id, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    [HttpPost("")]
    public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.AddAsync(pollId, request, cancellationToken);

        if (result.IsSuccess)
            return CreatedAtAction(nameof(Get), new { pollId, result.Value.Id }, result.Value);

        return result.ToProblem();

    }
    [HttpPut("{Id}")]
    public async Task<IActionResult> Update([FromRoute] int pollId, [FromRoute] int id, [FromBody] QuestionRequest request, CancellationToken cancellationToken)
    {
        var result = await _questionService.UpdateAsync(pollId, id, request, cancellationToken);

        if (result.IsSuccess)
            return NoContent();

        return result.ToProblem();

    }


    [HttpPut("{id}/ToggleStatus")]
    public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id, CancellationToken cancellationToken)
    {
        var result = await _questionService.ToggleStatusAsync(pollId, id, cancellationToken);

        return result.IsSuccess ? NoContent() : result.ToProblem();

    }

}
