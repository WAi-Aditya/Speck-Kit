using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class EmployeeSkillConfiguration : IEntityTypeConfiguration<EmployeeSkill>
{
    public void Configure(EntityTypeBuilder<EmployeeSkill> b)
    {
        b.ToTable("EmployeeSkills", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.ManagerNotes).HasMaxLength(2000);
        b.Property(x => x.SystemGeneratedScore).HasPrecision(18, 4);
        b.Property(x => x.FinalRating).HasPrecision(18, 4);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => x.EmployeeId);
        b.HasIndex(x => x.SkillId);
        b.HasIndex(x => new { x.EmployeeId, x.SkillId }).IsUnique();
        b.HasOne(x => x.Skill).WithMany().HasForeignKey(x => x.SkillId).OnDelete(DeleteBehavior.Restrict);
    }
}
