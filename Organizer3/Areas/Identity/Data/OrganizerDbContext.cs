using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Organizer3.Areas.Identity.Data;
using Organizer3.Models;
using Organizer3.Models.Recriter;
using Organizer3.Models.FacilitiesEditor;
using Organizer3.Models.EmployeeProfile;

namespace Organizer3.Data;

public class OrganizerDbContext : IdentityDbContext<AppUser>
{
    public OrganizerDbContext(DbContextOptions<OrganizerDbContext> options)
        : base(options)
    {
    }

    public DbSet<UserAccess> AccessPermisions { get; set; }
    public DbSet<AnnouncerModel> Announcements { get; set; }
    public DbSet<EmploymentStatus> EmploymentStatuses { get; set; }
    public DbSet<Facility> Facilities { get; set;}
    public DbSet<Recruitment> Recruitments { get; set; }
    public DbSet<RecruitmentNotes> recruitmentNotes { get; set; }
    public DbSet<Leave> Leaves { get; set; }
    public DbSet<SiteFunction> SiteFunctions { get; set; }
    public DbSet<ShiftInfo> ShiftInfos { get; set; }
    public DbSet<Atendance> Atendances { get; set; }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<AppUser>()
            .HasOne(a => a.Accesses)
            .WithOne(a => a.User)
            .HasForeignKey<UserAccess>(a => a.UserId);
        builder.Entity<AppUser>()
            .HasOne(a=>a.EmploymentStatus)
            .WithOne(a=>a.User)
            .HasForeignKey<EmploymentStatus>(a => a.UserId);
        builder.Entity<EmploymentStatus>()
            .HasOne(a=>a.Facility)
            .WithMany(a=>a.Employments)
            .HasForeignKey(a => a.FacilityId);

        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    //public DbSet<Organizer3.Models.JobAplicationModel> JobAplicationModel { get; set; }
    //public DbSet<Organizer3.Models.Recriter.AddRecruitmentNoteModel> AddRecruitmentNoteModel { get; set; }
    //public DbSet<Organizer3.Models.FacilitiesEditor.FacilitiesListModel>? FacilitiesListModel { get; set; }
    //public DbSet<Organizer3.Models.EmployeeProfile.AddNewLeaveModel>? AddNewLeaveModel { get; set; }
}
