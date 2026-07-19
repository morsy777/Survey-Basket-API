namespace SurveyBasket.Contracts.User;

public record UpdateUserRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password,
    IList<string> Roles
);
