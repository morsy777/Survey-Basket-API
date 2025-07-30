using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SurveyBasket.Controllers;

[Route("api/[controller]")] // = localHost/api/polls
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

    [HttpGet("{Id}")]
    public IActionResult Get([FromRoute] int Id)
    {
        var poll = _pollService.Get(Id);

        if (poll is null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();

        return Ok(response);
    }

    [HttpPost("")]
    public IActionResult Add([FromBody] CreatePollRequest request)
    {
        var newPoll = _pollService.Add(request.Adapt<Poll>());

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] CreatePollRequest request)
    {
        var isUpdated = _pollService.Update(id, request.Adapt<Poll>());

        if (!isUpdated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id) 
    {
        var isDeleted = _pollService.Delete(id);

        if(!isDeleted)
            return NotFound();

        return NoContent();
    }
}