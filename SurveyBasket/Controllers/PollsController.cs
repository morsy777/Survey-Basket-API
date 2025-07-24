namespace SurveyBasket.Controllers;

[Route("api/[controller]")] // = localHost/api/polls
[ApiController]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;


    [HttpGet("GetAll")]
    public IActionResult GetAll() => Ok(_pollService.GetAll());

    [HttpGet("{Id}")]
    public IActionResult Get(int Id) => _pollService.Get(Id) is null ? NotFound() : Ok(_pollService.Get(Id));

    [HttpPost("")]
    public IActionResult Add([FromBody] Poll request)
    {
        var newPoll = _pollService.Add(request);
        return CreatedAtAction(nameof(Get), new {id = newPoll.Id} ,newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Poll request)
    { 
        var isUpdated = _pollService.Update(id, request);

        if(!isUpdated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id) 
    {
        var isDeleted = _pollService.Delete(id);

        if(!isDeleted)
            return NotFound();

        return NoContent();
    }
}