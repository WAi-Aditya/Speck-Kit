using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class EmployeeSkillProjectConfiguration : IEntityTypeConfiguration<EmployeeSkillProject>
{
    public void Configure(EntityTypeBuilder<EmployeeSkillProject> b)
    {
        b.ToTable("EmployeeSkillProjects", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.RoleOrContribution).HasMaxLength(500);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasOne(x => x.EmployeeSkill).WithMany(x => x.Projects).HasForeignKey(x => x.EmployeeSkillId).OnDelete(DeleteBehavior.Cascade);
    }
}
