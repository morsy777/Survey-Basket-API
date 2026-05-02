namespace SurveyBasket.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class RolesController(IRoleService roleService) : ControllerBase
{
    private readonly IRoleService _roleService = roleService;

    [HttpGet("")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> GetAll([FromQuery] bool includeDisabled, CancellationToken cancellationToken = default)
    {
        var roles = await _roleService.GetAllAsync(includeDisabled, cancellationToken);

        return Ok(roles);
    }

    [HttpGet("{id}")]
    [HasPermission(Permissions.GetRoles)]
    public async Task<IActionResult> Get([FromRoute] string id, CancellationToken cancellationToken)
    {
        var result = await _roleService.GetAsync(id, cancellationToken);

        return result.IsSuccess ? Ok(result.Value) : result.ToProblem();
    }
}
