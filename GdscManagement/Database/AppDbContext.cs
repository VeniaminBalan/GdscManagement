using GdscManagement.Features.Roles;
using GdscManagement.Features.User;
using Microsoft.EntityFrameworkCore;

namespace GdscManagement.Database;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    
    
}