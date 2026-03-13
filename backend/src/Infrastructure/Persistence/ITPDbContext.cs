using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITP.Api.Infrastructure.Persistence;

public class ITPDbContext : DbContext
{
    public ITPDbContext(DbContextOptions<ITPDbContext> options) : base(options) { }

    public DbSet<SkillCategory> SkillCategories => Set<SkillCategory>();
    public DbSet<SkillSubCategory> SkillSubCategories => Set<SkillSubCategory>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<SkillLevelCriteria> SkillLevelCriteria => Set<SkillLevelCriteria>();
    public DbSet<EmployeeSkill> EmployeeSkills => Set<EmployeeSkill>();
    public DbSet<EmployeeSkillProject> EmployeeSkillProjects => Set<EmployeeSkillProject>();
    public DbSet<Certification> Certifications => Set<Certification>();
    public DbSet<Assessment> Assessments => Set<Assessment>();
    public DbSet<AssessmentResult> AssessmentResults => Set<AssessmentResult>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ITPDbContext).Assembly);
    }
}
