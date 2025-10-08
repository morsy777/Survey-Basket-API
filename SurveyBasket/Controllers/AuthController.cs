using Azure;

namespace SurveyBasket.Controllers;

[Route("[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("")]
    public async Task<IActionResult> LoginAsync(LoginRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);
        
        return authResult.IsSuccess
          ? Ok(authResult.Value)
          : authResult.ToProblem(StatusCodes.Status400BadRequest);
    }

    [HttpPost("refreshToken")]
    public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return authResult.IsSuccess 
            ? Ok(authResult.Value)
            : Problem(statusCode: StatusCodes.Status404NotFound, title: authResult.Error.Code, detail: authResult.Error.Description);
    }

    [HttpPut("Revoke-RefreshToken")]
    public async Task<IActionResult> RevokeRefreshToken([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        return isRevoked ? Ok() : BadRequest("Operation failed");
    }

    // Return Refresh Token in Cookies
    private void SetRefreshTokenCookies(string refreshToken, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires.ToLocalTime()
        };

        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}
