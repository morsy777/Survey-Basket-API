namespace SurveyBasket.Errors;

public static class UserErrors
{
    public static readonly Error InvalidCredentials =
        new Error("User.InvalidCredentials", "Invalid Email/Password");

    public static readonly Error InvalidTokens =
        new Error("User.InvalidTokens", "Invalid Token/Refresh Token");
}
