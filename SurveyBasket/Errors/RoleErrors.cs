namespace SurveyBasket.Errors;

public class RoleErrors
{
    public static readonly Error RoleNotFound =
        new Error("Role.RoleNotFound", "Role is not found", StatusCodes.Status404NotFound);

    public static readonly Error DuplicatedRole =
        new Error("Role.DuplicatedRole", "Another role with the same name already Exist", StatusCodes.Status409Conflict);

    public static readonly Error InvalidPermissions =
        new Error("User.InvalidPermissions", "Invalid permissions", StatusCodes.Status400BadRequest);

    //public static readonly Error InvalidRefreshToken =
    //    new Error("User.InvalidRefreshToken", "Invalid Refresh Token", StatusCodes.Status401Unauthorized);

    //public static readonly Error DuplicatedEmail =
    //    new Error("User.DuplicatedEmail", "Another User with the same Email already Exist", StatusCodes.Status409Conflict);

    //public static readonly Error EmailNotConfirmed =
    //    new Error("User.EmailNotConfirmed", "Email is not confirmed", StatusCodes.Status401Unauthorized);

    //public static readonly Error InvalidCode =
    //    new Error("User.InvalidCode", "Invalid confirmation code", StatusCodes.Status401Unauthorized);

    //public static readonly Error DuplicatedConfirmation =
    //    new Error("User.DuplicatedConfirmation", "The email already confrimed", StatusCodes.Status401Unauthorized);

}
