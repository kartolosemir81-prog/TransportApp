using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TransportApp.Models;

namespace TransportApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<RouteInfo> Routes { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
