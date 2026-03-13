using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class CertificationConfiguration : IEntityTypeConfiguration<Certification>
{
    public void Configure(EntityTypeBuilder<Certification> b)
    {
        b.ToTable("Certifications", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.Title).HasMaxLength(300).IsRequired();
        b.Property(x => x.Issuer).HasMaxLength(300).IsRequired();
        b.Property(x => x.DocumentBlobUrl).HasMaxLength(2000);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasIndex(x => x.EmployeeId);
        b.HasIndex(x => x.SkillId);
        b.HasOne(x => x.Skill).WithMany().HasForeignKey(x => x.SkillId).OnDelete(DeleteBehavior.Restrict);
    }
}
