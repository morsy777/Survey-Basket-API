namespace SurveyBasket.Contracts.Roles;

public record RoleDetialResponse(
    string Id,
    string Name,
    bool IsDeleted,
    IEnumerable<string> Permissions
);