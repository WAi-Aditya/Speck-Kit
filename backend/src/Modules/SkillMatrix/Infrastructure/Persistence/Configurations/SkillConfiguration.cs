using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class SkillConfiguration : IEntityTypeConfiguration<Skill>
{
    public void Configure(EntityTypeBuilder<Skill> b)
    {
        b.ToTable("Skills", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.Name).HasMaxLength(200).IsRequired();
        b.Property(x => x.Description).HasMaxLength(1000);
        b.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => new { x.SkillSubCategoryId, x.Name }).IsUnique();
        b.HasOne(x => x.SkillSubCategory).WithMany(x => x.Skills).HasForeignKey(x => x.SkillSubCategoryId).OnDelete(DeleteBehavior.Restrict);

        // Mock seed data: sample skills (constitution P1 examples)
        var now = new DateTimeOffset(2026, 1, 1, 0, 0, 0, TimeSpan.Zero);
        b.HasData(
            new Skill { Id = Guid.Parse("B4000001-0000-0000-0000-000000000001"), SkillSubCategoryId = Guid.Parse("B3000001-0000-0000-0000-000000000001"), Name = "React", Description = "React.js framework", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now },
            new Skill { Id = Guid.Parse("B4000002-0000-0000-0000-000000000002"), SkillSubCategoryId = Guid.Parse("B3000002-0000-0000-0000-000000000002"), Name = "Cloud Services", Description = "Azure/AWS cloud services", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now },
            new Skill { Id = Guid.Parse("B4000003-0000-0000-0000-000000000003"), SkillSubCategoryId = Guid.Parse("B3000003-0000-0000-0000-000000000003"), Name = "Manual Testing", Description = "Manual QA and testing", IsActive = true, SortOrder = 1, CreatedAt = now, UpdatedAt = now }
        );
    }
}
