using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ITP.Api.Modules.SkillMatrix.Infrastructure.Persistence.Configurations;

public class AssessmentResultConfiguration : IEntityTypeConfiguration<AssessmentResult>
{
    public void Configure(EntityTypeBuilder<AssessmentResult> b)
    {
        b.ToTable("AssessmentResults", "SkillMatrix");
        b.HasKey(x => x.Id);
        b.Property(x => x.Score).HasPrecision(18, 4).IsRequired();
        b.Property(x => x.Comments).HasMaxLength(2000);
        b.Property(x => x.CreatedAt).IsRequired();
        b.Property(x => x.UpdatedAt).IsRequired();
        b.Property(x => x.CreatedBy).HasMaxLength(450);
        b.Property(x => x.UpdatedBy).HasMaxLength(450);
        b.HasOne(x => x.Assessment).WithMany(x => x.Results).HasForeignKey(x => x.AssessmentId).OnDelete(DeleteBehavior.Cascade);
    }
}
