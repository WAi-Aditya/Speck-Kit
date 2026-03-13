using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> b)
    {
        b.ToTable("Assessments", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => x.EmployeeSkillId);
        b.HasOne(x => x.EmployeeSkill).WithMany(x => x.Assessments).HasForeignKey(x => x.EmployeeSkillId).OnDelete(DeleteBehavior.Cascade);
    }
}
