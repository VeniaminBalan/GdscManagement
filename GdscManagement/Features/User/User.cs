using GdscManagement.Base.Models;
using GdscManagement.Features.Roles;

namespace GdscManagement.Features.User;

public class User : Entity
{
    public string FirstName { get; set; }
    
    public string LastName  { get; set; }
    
    public string Email  { get; set; }

    public List<Role> Roles { get; set; }
}