using GdscManagement.Features.Roles;

namespace GdscManagement.Features.User;

public class UserResponse
{
    public string id { get; set; }
    public string FirstName { get; set; }
    public string LastName  { get; set; }
    public string Email  { get; set; }
    public IEnumerable<RoleResponseForUser> Roles { get; set; }
}