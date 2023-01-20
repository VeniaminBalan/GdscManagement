using System.ComponentModel.DataAnnotations;

namespace GdscManagement.Features.Teams;

public class TeamRequest
{
    [Required]public string TeamLead { get; set; }
    [Required]public string Name { get; set; }
    [Required]public string GitHubLink { get; set; }
}