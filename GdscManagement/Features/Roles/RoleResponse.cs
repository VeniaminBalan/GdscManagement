

using GdscManagement.Features.User;

namespace GdscManagement.Features.Roles;

public class RoleResponse
{
    public string Value { get; set; }
    public string Description { get; set; }
    public IEnumerable<UserResponeForRole> Users { get; set; }
}