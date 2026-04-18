using Microsoft.AspNetCore.OutputCaching;

namespace SurveyBasket.Controllers;

[Route("api/polls/{pollId}/vote")]
[ApiController]
//[Authorize]
public class VotesController(IQuestionService questionService, IVoteService voteService) : ControllerBase
{
    private readonly IQuestionService _questionService = questionService;
    private readonly IVoteService _voteService = voteService;

    [HttpGet("")]
    public async Task<IActionResult> Start([FromRoute] int pollId, CancellationToken cancellationToken)
    {
        var userId = "96f53559-46d5-41d7-96e3-6e2f935422d0";
        var result = await _questionService.GetAvailableAsync(pollId, userId, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
    
    [HttpPost("")]
    public async Task<IActionResult> Vote([FromRoute] int pollId, [FromBody] VoteRequest request, CancellationToken cancellationToken)
    {
        var result = await _voteService.AddAsync(pollId, User.GetUserId()!, request, cancellationToken);

        return result.IsSuccess ? Created() : result.ToProblem();
    }


}
