using ITP.Api.Modules.SkillMatrix.Domain.Common;

namespace ITP.Api.Modules.SkillMatrix.Domain.Entities;

/// <summary>
/// Top-level skill category (e.g. Development, Cloud, QA). Constitution P1.
/// </summary>
public class SkillCategory : AuditableEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public ICollection<SkillSubCategory> SubCategories { get; set; } = new List<SkillSubCategory>();
}

/// <summary>
/// Sub-category under a category (e.g. Frontend under Development). Constitution P1.
/// </summary>
public class SkillSubCategory : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid SkillCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public SkillCategory SkillCategory { get; set; } = null!;
    public ICollection<Skill> Skills { get; set; } = new List<Skill>();
}

/// <summary>
/// Leaf skill under a sub-category (e.g. React under Frontend). Constitution P1.
/// </summary>
public class Skill : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid SkillSubCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    public int SortOrder { get; set; }

    public SkillSubCategory SkillSubCategory { get; set; } = null!;
}

/// <summary>
/// Criteria/label for each proficiency level (Beginner, Intermediate, Advanced, Expert). Used for display and optional thresholds.
/// </summary>
public class SkillLevelCriteria : AuditableEntity
{
    public Guid Id { get; set; }
    public int SkillLevel { get; set; } // 1–4
    public string Label { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? MinScore { get; set; }
}
