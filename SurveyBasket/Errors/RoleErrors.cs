namespace SurveyBasket.Errors;

public class RoleErrors
{
    public static readonly Error RoleNotFound =
        new Error("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);
}
