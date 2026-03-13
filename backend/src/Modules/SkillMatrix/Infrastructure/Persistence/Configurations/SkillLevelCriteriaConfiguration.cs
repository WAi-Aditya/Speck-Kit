using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class SkillLevelCriteriaConfiguration : IEntityTypeConfiguration<SkillLevelCriteria>
{
    public void Configure(EntityTypeBuilder<SkillLevelCriteria> b)
    {
        b.ToTable("SkillLevelCriteria", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.SkillLevel).IsRequired();
        b.Property(x => x.Label).HasMaxLength(100).IsRequired();
        b.Property(x => x.Description).HasMaxLength(500);
        b.Property(x => x.MinScore).HasPrecision(18, 4);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => x.SkillLevel).IsUnique();

        // Seed baseline proficiency levels (constitution P2)
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        b.HasData(
            new SkillLevelCriteria { Id = Guid.Parse("A1000001-0000-0000-0000-000000000001"), SkillLevel = 1, Label = "Beginner", Description = "Basic understanding; requires guidance and supervision", CreatedAt = now, UpdatedAt = now },
            new SkillLevelCriteria { Id = Guid.Parse("A1000002-0000-0000-0000-000000000002"), SkillLevel = 2, Label = "Intermediate", Description = "Works independently on standard tasks with confidence", CreatedAt = now, UpdatedAt = now },
            new SkillLevelCriteria { Id = Guid.Parse("A1000003-0000-0000-0000-000000000003"), SkillLevel = 3, Label = "Advanced", Description = "Handles complex tasks; mentors others effectively", CreatedAt = now, UpdatedAt = now },
            new SkillLevelCriteria { Id = Guid.Parse("A1000004-0000-0000-0000-000000000004"), SkillLevel = 4, Label = "Expert", Description = "Subject-matter expert; drives innovation and strategy", CreatedAt = now, UpdatedAt = now }
        );
    }
}
