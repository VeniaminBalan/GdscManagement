using System.ComponentModel.DataAnnotations;
using GdscManagement.Features.Roles;

namespace GdscManagement.Features.User;

public class UserRequest
{
    [Required]public string FirstName { get; set; }
    [Required]public string LastName  { get; set; }
    
    [EmailAddress]
    [Required]
    public string Email  { get; set; }
}