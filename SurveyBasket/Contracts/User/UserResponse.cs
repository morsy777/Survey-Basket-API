namespace SurveyBasket.Contracts.User;

public record UserResponse(
    string Id,
    string firstName,
    string LastName,
    string Email,
    bool IsDisabled,
    IEnumerable<string> Roles
);