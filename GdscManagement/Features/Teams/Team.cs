using GdscManagement.Base.Models;

namespace GdscManagement.Features.Teams;

public class Team : Entity
{
    public string TeamLead { get; set; }
    public string Name { get; set; }
    public string GitHubLink { get; set; }
}