using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Errors;
using SurveyBasket.Helpers;
using System.Security.Cryptography;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace SurveyBasket.Services;

public class AuthService(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IJwtProvider jwtProvider,
    ILogger<AuthService> logger,
    IEmailSender emailSender,
    IHttpContextAccessor httpContextAccessor) : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IJwtProvider _jwtProvider = jwtProvider;
    private readonly ILogger<AuthService> _logger = logger;
    private readonly IEmailSender _emailSender = emailSender;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    private readonly int _refreshTokenExpireDays = 14;

    public async Task<Result<AuthResponse>> GetTokenAsync(string email, string password,
        CancellationToken cancellation = default)
    {
        // Check user by using email?
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

        var result = await _signInManager.PasswordSignInAsync(user, password, false, false);

        if (result.Succeeded)
        {
            // Generate JWT Token
            var (token, expiresIn) = _jwtProvider.GenerateToken(user);

            // Call Refresh Token Genrator Function
            var refreshToken = GenerateRefreshToke();
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

            // Add Refresh Token to DB
            user.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshToken,
                ExpiresOn = refreshTokenExpiration
            });

            await _userManager.UpdateAsync(user);

            // Return Token & Refresh Token
            var response = new AuthResponse(
                    user.Id,
                    user.Email,
                    user.FirstName,
                    user.LastName,
                    token,
                    expiresIn,
                    refreshToken,
                    refreshTokenExpiration
            );

            return Result.Success(response);
        }

        return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);
    }

    public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

        // Revoke the old refresh token
        userRefreshToken.RevokedOn = DateTime.UtcNow;

        // Generate New JWT Token
        var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);

        // Call Refresh Token Genrator Function to Generate New refresh token
        var newRefreshToken = GenerateRefreshToke();
        var refreshTokenExpiration = DateTime.UtcNow.AddDays(_refreshTokenExpireDays);

        // Add Refresh Token to DB
        user.RefreshTokens.Add(new RefreshToken
        {
            Token = newRefreshToken,
            ExpiresOn = refreshTokenExpiration
        });

        await _userManager.UpdateAsync(user);

        // Return Token & Refresh Token
        var response = new AuthResponse(
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                newToken,
                expiresIn,
                newRefreshToken,
                refreshTokenExpiration
        );

        return Result.Success<AuthResponse>(response);
    }

    // Generate Refresh Token
    private string GenerateRefreshToke()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    public async Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellation = default)
    {
        var userId = _jwtProvider.ValidateToken(token);

        if (userId is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return Result.Failure(UserErrors.InvalidJwtToken);

        var userRefreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken && x.IsActive);

        if (userRefreshToken is null)
            return Result.Failure(UserErrors.InvalidRefreshToken);

        userRefreshToken.RevokedOn = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return Result.Success();
    }

    public async Task<Result> RegisterAsync(RegisterRequest request,CancellationToken cancellation = default)
    {
        // TODO: Check if the email already exists
        var emailIsExist = await _userManager.Users.AnyAsync(x => x.Email == request.Email, cancellation);

        // TODO: If not exist, map the registration model to the ApplicationUser entity
        if (emailIsExist)
            return Result.Failure<AuthResponse>(UserErrors.DuplicatedEmail);

        var user = request.Adapt<ApplicationUser>();

        // TODO: Create the new user using UserManager
        var result = await _userManager.CreateAsync(user, request.Password);

        // TODO: If creation succeeded, generate Access Token, Refresh Token and return them
        if(result.Succeeded)
        {
            // TODO: Generat Email Confirmation Token and Encode it to BaseURL
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            // TODO: Test The Code using Logging
            _logger.LogInformation($"Confirmation code: {code}");

            // TODO: Send email
            await SendConfirmationEmail(user, code);

            return Result.Success();
        }

        // TODO: Return Failure
        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status401Unauthorized));
    }

    public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
    {
        // TODO: Check UserId by using UserManager
        if(await _userManager.FindByIdAsync(request.UserId) is not { } user)
            return Result.Failure(UserErrors.InvalidCode);

        // TODO: Check if the email is already confirmed
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        // TODO: Decode the code by using WebEncoder.Base64UrlDecode(code) and then convert it to string
        var code = request.Code;

        try
        {
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Code));
        }
        catch (FormatException)
        {
            Result.Failure(UserErrors.InvalidCode);
        }

        // TODO: Confrim the email by using UserManager
        var result = await _userManager.ConfirmEmailAsync(user, code);

        // TODO: If the email confirmed successfully, return Success else return Failure
        if(result.Succeeded) 
            return Result.Success();

        var error = result.Errors.First();

        return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }

    public async Task<Result> ResendConfirmationEmailAsync(ResendConfirmationEmailRequest request)
    {
        // TODO: Find user by using email
        if(await _userManager.FindByEmailAsync(request.Email) is not { } user)
            return Result.Success();

        // TODO: Check if email already confirmed
        if (user.EmailConfirmed)
            return Result.Failure(UserErrors.DuplicatedConfirmation);

        // TODO: Generate new code 
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

        // TODO: logg it for testing
        _logger.LogInformation(code);

        // TODO: Send Email
        await SendConfirmationEmail(user, code);

        return Result.Success();
    }

    private async Task SendConfirmationEmail(ApplicationUser user, string code)
    {
        // TODO: Take the frontend origin from the request headers
        var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

        // TODO: Generate email body
        var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
            templateModel: new Dictionary<string, string>
            {
                    {"{{name}}", user.FirstName},
                    {"{{action_url}}", $"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"}
            }

        );

        // TODO: Send the email
        await _emailSender.SendEmailAsync(user.Email!, "Survey Basket: Email Confirmation", emailBody);
    }

}
