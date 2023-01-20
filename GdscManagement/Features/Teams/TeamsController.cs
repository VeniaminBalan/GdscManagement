using GdscManagement.Database;
using Microsoft.AspNetCore.Mvc;

namespace GdscManagement.Features.Teams;

[ApiController]
[Route("teams")]
public class TeamsController : ControllerBase
{
    private readonly AppDbContext _appDbContext;

    public TeamsController(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }
    
    
}