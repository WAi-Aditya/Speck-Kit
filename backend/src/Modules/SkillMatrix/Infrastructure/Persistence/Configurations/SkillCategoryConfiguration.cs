using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class SkillCategoryConfiguration : IEntityTypeConfiguration<SkillCategory>
{
    public void Configure(EntityTypeBuilder<SkillCategory> b)
    {
        b.ToTable("SkillCategories", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(1000);
        b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => x.Name).IsUnique();

        // Mock seed data: sample categories (constitution P1 examples)
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        b.HasData(
            new SkillCategory { Id = Guid.Parse("B2000001-0000-0000-0000-000000000001"), Name = "Development", Description = "Software development and engineering", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now },
            new SkillCategory { Id = Guid.Parse("B2000002-0000-0000-0000-000000000002"), Name = "Cloud", Description = "Cloud platforms and infrastructure", IsActive = true, SortOrder = 2, CreatedAt = now, UpdatedAt = now },
            new SkillCategory { Id = Guid.Parse("B2000003-0000-0000-0000-000000000003"), Name = "QA", Description = "Quality assurance and testing", IsActive = true, SortOrder = 3, CreatedAt = now, UpdatedAt = now }
        );
    }
}
