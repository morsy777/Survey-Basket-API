namespace SurveyBasket.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleResponse>> GetAllAsync(bool? includeDisabled = false, CancellationToken cancellationToken = default);
    Task<Result<RoleDetialResponse>> GetAsync(string RoleId, CancellationToken cancellationToken = default);
    Task<Result<RoleDetialResponse>> AddAsync(RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> UpdateAsync(string id, RoleRequest request, CancellationToken cancellationToken = default);
    Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default);
}
