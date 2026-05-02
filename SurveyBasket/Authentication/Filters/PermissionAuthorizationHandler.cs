namespace SurveyBasket.Authentication.Filters;

public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        // Check, is user authenticated & is the permission required exist in the user claims

        //var user = context.User.Identity;
        //if (user is null || !user.IsAuthenticated)
        //    return;

        //var hasPermission = !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type);
        //if (hasPermission)
        //    return;

        if (context.User.Identity is not { IsAuthenticated: true } ||
            !context.User.Claims.Any(x => x.Value == requirement.Permission && x.Type == Permissions.Type))
            return;

        context.Succeed(requirement);
        return;
    }
}
