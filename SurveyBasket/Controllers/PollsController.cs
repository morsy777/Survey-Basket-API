namespace SurveyBasket.Controllers;

[Route("api/[controller]")] // = localHost/api/polls
[ApiController]
[Authorize]
public class PollsController(IPollService pollService) : ControllerBase
{
    private readonly IPollService _pollService = pollService;

    [HttpGet("GetAll")]
    [EnableCors("MyPolicy2")]
    public async Task<IActionResult> GetAll(CancellationToken cancellation)
    {
        var polls = await _pollService.GetAllAsync(cancellation);

        var response = polls.Adapt<IEnumerable<PollResponse>>();

        return Ok(response);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> Get([FromRoute] int Id,
        CancellationToken cancellationToken)
    {
        var poll = await _pollService.GetAsync(Id, cancellationToken);

        if (poll is null)
            return NotFound();

        var response = poll.Adapt<PollResponse>();

        return Ok(response);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request.Adapt<Poll>(), cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Id }, newPoll.Adapt<PollResponse>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

        if (!isUpdated)
            return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

        if (!isDeleted)
            return NotFound();

        return NoContent();
    }

    [HttpPut("{id}/toggleIsPublish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        if (!isUpdated)
            return NotFound();

        return NoContent();
    }
}