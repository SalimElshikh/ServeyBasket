namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        var polls = _pollService.GetAll();
        var response = polls.Adapt<IEnumerable<PollResponse>>();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute]int id)
    {
        var poll = _pollService.GetById(id);

        if (poll is null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();



        return Ok(response);
    } 

    [HttpPost("")]
    public IActionResult Add([FromBody]PollRequest request)
    {
        var newPoll = _pollService.Add(request.Adapt<Poll>());

        return CreatedAtAction(nameof(GetById),new { Id = newPoll.Id } , newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute]int id,[FromBody]PollRequest request)
    {
        
        if(!_pollService.Update(id, request.Adapt<Poll>()))
            return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute]int id)
    {
        if(!_pollService.Delete(id))
            return NotFound();
        return NoContent();

    }

}
