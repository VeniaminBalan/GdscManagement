using GdscManagement.Database;
using Microsoft.AspNetCore.Mvc;

namespace GdscManagement.Features.Roles;

[ApiController]
[Route("roles")]
public class RolesController : ControllerBase
{
    private readonly AppDbContext _appDbContext;
    
    public RolesController(AppDbContext appDbContext )
    {
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public async Task<ActionResult<Role>> AddRole(RoleRequest roleRequest)
    {
        var role = new Role()
        {
            id = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            Value = roleRequest.Value,
            Description = roleRequest.Description
            
        };
        
        var result = await _appDbContext.Roles.AddAsync(role);
        await _appDbContext.SaveChangesAsync();

        return Created("role", result.Entity);
    }
    
    
}