namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;


    [HttpGet("GetAll")]
    public IActionResult GetAll()
    {
        return Ok(_pollService.GetAll());
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var poll = _pollService.GetById(id);
        return poll is null ? NotFound(): Ok(poll);
    }

    [HttpPost("")]
    public IActionResult Add(Poll request)
    {
        var newPoll = _pollService.Add(request);

        return CreatedAtAction(nameof(GetById),new { Id = newPoll.Id } , newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id,Poll request)
    {
        
        if(!_pollService.Update(id, request))
            return NotFound();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        if(!_pollService.Delete(id))
            return NotFound();
        return NoContent();

    }

}
