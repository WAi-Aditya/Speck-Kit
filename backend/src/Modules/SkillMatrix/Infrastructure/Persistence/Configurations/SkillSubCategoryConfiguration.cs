using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class SkillSubCategoryConfiguration : IEntityTypeConfiguration<SkillSubCategory>
{
    public void Configure(EntityTypeBuilder<SkillSubCategory> b)
    {
        b.ToTable("SkillSubCategories", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(1000);
        b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => new { x.SkillCategoryId, x.Name }).IsUnique();
        b.HasOne(x => x.SkillCategory).WithMany(x => x.SubCategories).HasForeignKey(x => x.SkillCategoryId).OnDelete(DeleteBehavior.Restrict);

        // Mock seed data: sample subcategories
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        b.HasData(
            new SkillSubCategory { Id = Guid.Parse("B3000001-0000-0000-0000-000000000001"), SkillCategoryId = Guid.Parse("B2000001-0000-0000-0000-000000000001"), Name = "Frontend", Description = "Frontend development", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now },
            new SkillSubCategory { Id = Guid.Parse("B3000002-0000-0000-0000-000000000002"), SkillCategoryId = Guid.Parse("B2000002-0000-0000-0000-000000000002"), Name = "Azure / AWS", Description = "Cloud services", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now },
            new SkillSubCategory { Id = Guid.Parse("B3000003-0000-0000-0000-000000000003"), SkillCategoryId = Guid.Parse("B2000003-0000-0000-0000-000000000003"), Name = "Automation", Description = "Test automation", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now }
        );
    }
}
