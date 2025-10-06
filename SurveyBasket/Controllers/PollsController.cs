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
        var result = await _pollService.GetAsync(Id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
    }

    [HttpPost("")]
    public async Task<IActionResult> Add([FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        var newPoll = await _pollService.AddAsync(request, cancellationToken);

        return CreatedAtAction(nameof(Get), new { id = newPoll.Value.Id }, newPoll.Value.Adapt<PollResponse>());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest request,
        CancellationToken cancellationToken)
    {
        //var isUpdated = await _pollService.UpdateAsync(id, request.Adapt<Poll>(), cancellationToken);

        //if (!isUpdated)
        //    return NotFound();

        //return NoContent();

        var result = await _pollService.UpdateAsync(id, request, cancellationToken);

        return result.IsSuccess 
            ? NoContent()
            : NotFound(result.Error);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        //var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

        //if (!isDeleted)
        //    return NotFound();

        //return NoContent();

        var result = await _pollService.DeleteAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : NotFound(result.Error);
    }

    [HttpPut("{id}/toggleIsPublish")]
    public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, CancellationToken cancellationToken)
    {
        //var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        //if (!isUpdated)
        //    return NotFound();

        //return NoContent();

        var result = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

        return result.IsSuccess
            ? NoContent()
            : NotFound(result.Error);
    }
}