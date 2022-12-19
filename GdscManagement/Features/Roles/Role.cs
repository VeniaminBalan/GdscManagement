using GdscManagement.Base.Models;

namespace GdscManagement.Features.Roles;

public class Role : Entity
{
    public string Value { get; set; }
    public string Description { get; set; }

    public IEnumerable<User.User> Users { get; set; }
}