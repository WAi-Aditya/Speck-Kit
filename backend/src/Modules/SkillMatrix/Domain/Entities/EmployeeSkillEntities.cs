using ITP.Api.Modules.SkillMatrix.Domain.Common;
using ITP.Api.Modules.SkillMatrix.Domain.Enums;

namespace ITP.Api.Modules.SkillMatrix.Domain.Entities;

/// <summary>
/// Employee's assignment of a skill with self/manager/system ratings and validation status. Constitution P2, P3.
/// </summary>
public class EmployeeSkill : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid SkillId { get; set; }
    public int? SelfAssessedLevel { get; set; } // 1–4
    public int? ManagerValidatedLevel { get; set; }
    public decimal? SystemGeneratedScore { get; set; }
    public decimal? FinalRating { get; set; }
    public ValidationStatus ValidationStatus { get; set; } = ValidationStatus.Pending;
    public string? ManagerNotes { get; set; }
    public DateTimeOffset? ValidatedAt { get; set; }
    public Guid? ValidatedBy { get; set; }

    public Skill Skill { get; set; } = null!;
    public ICollection<EmployeeSkillProject> Projects { get; set; } = new List<EmployeeSkillProject>();
    public ICollection<Assessment> Assessments { get; set; } = new List<Assessment>();
}

/// <summary>
/// Project evidence linked to an employee skill (contribution/role on a project).
/// </summary>
public class EmployeeSkillProject : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EmployeeSkillId { get; set; }
    public Guid ProjectId { get; set; }
    public string? RoleOrContribution { get; set; }
    public DateTimeOffset? StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }

    public EmployeeSkill EmployeeSkill { get; set; } = null!;
}
