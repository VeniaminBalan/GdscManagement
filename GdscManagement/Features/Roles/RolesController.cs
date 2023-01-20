using GdscManagement.Database;
using GdscManagement.Features.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleResponse>>> GetRole()
    {
        var roles = await _appDbContext.Roles.Select(
            role => new RoleResponse
            {
                Value = role.Value,
                Description = role.Description,
                Users = role.Users.Select(user => new UserResponeForRole
                    {
                        id = user.id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        Email = user.Email,
                    })
            }
        ).ToListAsync();

        return Ok(roles);
    }
}