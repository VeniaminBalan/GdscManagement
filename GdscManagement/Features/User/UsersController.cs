using GdscManagement.Database;
using GdscManagement.Features.Roles;
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

        var res =  users.FirstOrDefault(x => x.id == id);
        if (res is null) return NotFound("this user does not exist");

        return res;
    }
}