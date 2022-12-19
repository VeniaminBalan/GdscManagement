using System.ComponentModel.DataAnnotations;

namespace GdscManagement.Features.Roles;

public class RoleRequest
{
    [Required]public string Value { get; set; }
    [Required]public string Description { get; set; }
}