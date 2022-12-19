

namespace GdscManagement.Features.Roles;

public class RoleResponse
{
    public string Value { get; set; }
    public string Description { get; set; }
    public IEnumerable<User.User> Users { get; set; }
}