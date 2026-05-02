namespace SurveyBasket.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;

    // (includeDisabled.HasValue && includeDisabled.Value) 
    // mean includeDisabled == true
    public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default) =>
        await _roleManager.Roles
            .Where(x => !x.IsDefault && (!x.IsDeleted || (includeDisabled.HasValue && includeDisabled.Value)))
            .ProjectToType<RoleResponse>()
            .ToListAsync(cancellationToken);

    public async Task<Result<RoleDetialResponse>> GetAsync(string RoleId, CancellationToken cancellationToken = default)
    {
        if(await _roleManager.FindByIdAsync(RoleId) is not { } role)
            return Result.Failure<RoleDetialResponse>(RoleErrors.RoleNotFound);

        var permissions = await _roleManager.GetClaimsAsync(role);

        var response = new RoleDetialResponse(
            RoleId,
            role.Name!,
            role.IsDeleted,
            permissions.Select(x => x.Value)
        );

        return Result.Success(response);
    }
}
