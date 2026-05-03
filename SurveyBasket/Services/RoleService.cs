using System.Linq;

namespace SurveyBasket.Services;

public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
{
    private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
    private readonly ApplicationDbContext _context = context;

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

    public async Task<Result<RoleDetialResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default)
    {
        var roleIsExist = await _roleManager.RoleExistsAsync(request.Name);

        if(roleIsExist)
            return Result.Failure<RoleDetialResponse>(RoleErrors.DuplicatedRole);

        var allowedPermissions = Permissions.GetAllPermissions();

        if (request.Permissions.Except(allowedPermissions).Any())
            return Result.Failure<RoleDetialResponse>(RoleErrors.InvalidPermissions);

        var role = new ApplicationRole
        {
            Name = request.Name,
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        var result = await _roleManager.CreateAsync(role);

        if(result.Succeeded)
        {
            var permissions = request.Permissions
                .Select(x => new IdentityRoleClaim<string>
                {
                    ClaimType = Permissions.Type,
                    ClaimValue = x,
                    RoleId = role.Id
                });

            await _context.AddRangeAsync(permissions, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            var response = new RoleDetialResponse(
                    role.Id,
                    role.Name!,
                    role.IsDeleted,
                    request.Permissions
            );

            return Result.Success(response);
        }

        var error = result.Errors.First();

        return Result.Failure<RoleDetialResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
    }
}
