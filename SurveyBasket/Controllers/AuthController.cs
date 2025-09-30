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

        if (authResult is not null && !string.IsNullOrEmpty(authResult.RefreshToken))
            SetRefreshTokenCookies(authResult.RefreshToken, authResult.RefreshTokenExpiration);


        return authResult is null ? BadRequest("Invalid Email or Password.") : Ok(authResult);
    }

    [HttpPost("refreshToke")]
    public async Task<IActionResult> RefreshAsync(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

        if (authResult is not null && !string.IsNullOrEmpty(authResult.RefreshToken))
            SetRefreshTokenCookies(authResult.RefreshToken, authResult.RefreshTokenExpiration);

        return authResult is null ? BadRequest("Invalid Email or Password.") : Ok(authResult);

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
