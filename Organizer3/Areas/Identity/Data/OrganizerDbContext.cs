using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;

namespace Organizer3.Data;

public class OrganizerDbContext : IdentityDbContext<AppUser>
{
    public OrganizerDbContext(DbContextOptions<OrganizerDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserAccess> AccessPermisions { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .HasOne(a => a.Accesses)
            .WithOne(a => a.User)
            .HasForeignKey<UserAccess>(a => a.UserId);
            

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
