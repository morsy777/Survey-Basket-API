namespace SurveyBasket.Controllers;

[Route("me")]
[ApiController]
[Authorize]
public class AccountController(IUserService userService) : ControllerBase
{
    private readonly IUserService _userService = userService;

    [HttpGet("")]
    public async Task<IActionResult> Info()
    {
        var result = await _userService.GetProfileAsync(User.GetUserId()!);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }

    [HttpPut("info")]
    public async Task<IActionResult> Info([FromBody] UpdateProfileRequest request)
    {
        var result = await _userService.UpdateProfileAsync(User.GetUserId()!, request);

        return NoContent(); 
    }
}
