using GdscManagement.Database;
using GdscManagement.Features.Roles;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GdscManagement.Features.User;

[ApiController]
[Route("user")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public UserController(AppDbContext appDbContext )
    {
        _appDbContext = appDbContext;
    }

    [HttpPost]
    public async Task<ActionResult<User>> AddUser(UserRequest userRequest)
    {
        var memberRole = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Value == "Member");
        if (memberRole is null) return NotFound("Member role does not exist");

        var roles = new List<Role>
        {
            memberRole
        };
        
        var user = new User
        {
            id = Guid.NewGuid().ToString(),
            Created = DateTime.UtcNow,
            Updated = DateTime.UtcNow,
            FirstName = userRequest.FirstName,
            LastName = userRequest.LastName,
            Email = userRequest.Email,
            Roles = roles
        };

        user = (await _appDbContext.Users.AddAsync(user)).Entity;
        await _appDbContext.SaveChangesAsync();
        
        var res = new UserResponse
        {
            id = Guid.NewGuid().ToString(),
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles.Select(
                role => new RoleResponseForUser()
                {
                    Value = role.Value,
                    Description = role.Description
                })
        };

        return Created("user", res);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUser()
    {
        var users = await _appDbContext.Users.Include(x => x.Roles).Select(
            user => new UserResponse
            {
                id = user.id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = user.Roles.Select(
                    role => new RoleResponseForUser()
                    {
                        Value = role.Value,
                        Description = role.Description
                    }
                )
            }).ToListAsync();


        return Ok(users);
    }
    
    
    [HttpGet("{id}")]
    public async Task<ActionResult<UserResponse>> GetUser([FromRoute] string id)
    {
        var user = await _appDbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => id == x.id);
        if(user is null) return  NotFound("User does not exist");

        var res = new UserResponse
        {
            id = user.id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            Roles = user.Roles.Select(
                role => new RoleResponseForUser
                {
                    Value = role.Value,
                    Description = role.Description
                }).ToList()
        };

        return res;
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<UserResponse>> UpdateUser([FromBody]UserRequest userRequest, [FromRoute] string id)
    {
        var user = await _appDbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => id == x.id);
        if(user is null) return  NotFound("User does not exist");

        user.FirstName = userRequest.FirstName;
        user.LastName = userRequest.LastName;
        user.Email = userRequest.Email;
        user.Updated = DateTime.UtcNow;

        await _appDbContext.SaveChangesAsync();

        return Ok(new UserResponse
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            id = user.id,
            Roles = user.Roles.Select(
                role => new RoleResponseForUser
                {
                    Value = role.Value,
                    Description = role.Description
                }).ToList()
        });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<UserResponse>> Delete([FromRoute] string id)
    {
        var user = await _appDbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => id == x.id);
        if (user is null) return NotFound("User was not found");

        _appDbContext.Remove(user);
        await _appDbContext.SaveChangesAsync();

        return Ok("user was deleted");
    }

    /*
    [HttpPatch("{id}")]
    public async Task<ActionResult<UserResponse>> AddRole([FromBody] RoleRequest roleRequest, [FromRoute] string id)
    {
        var user = await _appDbContext.Users.Include(x => x.Roles).FirstOrDefaultAsync(x => id == x.id);
        if(user is null) return  NotFound("User does not exist");
        
        var memberRole = await _appDbContext.Roles.FirstOrDefaultAsync(x => x.Value == roleRequest.Value);
        if (memberRole is null) return NotFound("Member role does not exist");

        
        
        return Ok("User and member role was found");
    }
    */
}