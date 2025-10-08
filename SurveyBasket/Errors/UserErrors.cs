namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new Error("User.InvalidCredentials", "Invalid Email/Password", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidJwtToken =
        new Error("User.InvalidJwtToken", "Invalid Jwt Token", StatusCodes.Status400BadRequest);

    public static readonly Error InvalidRefreshToken =
        new Error("User.InvalidRefreshToken", "Invalid Refresh Token", StatusCodes.Status400BadRequest);

    public static readonly Error DuplicatedEmail =
        new Error("User.DuplicatedEmail", "Another User with the same Email already Exist", StatusCodes.Status400BadRequest);
}
