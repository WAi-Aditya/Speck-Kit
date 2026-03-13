using ITP.Api.Modules.SkillMatrix.Domain.Common;
using ITP.Api.Modules.SkillMatrix.Domain.Enums;

namespace ITP.Api.Modules.SkillMatrix.Domain.Entities;

/// <summary>
/// Certification/credential linked to a skill. Constitution P3.
/// </summary>
public class Certification : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public Guid SkillId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public DateTimeOffset IssueDate { get; set; }
    public DateTimeOffset? ExpiryDate { get; set; }
    public string? DocumentBlobUrl { get; set; }

    public Skill Skill { get; set; } = null!;
}

/// <summary>
/// An assessment (self, manager, peer, system) for an employee skill.
/// </summary>
public class Assessment : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EmployeeSkillId { get; set; }
    public AssessmentType AssessmentType { get; set; }
    public AssessmentStatus Status { get; set; } = AssessmentStatus.Assigned;
    public DateTimeOffset? DueDate { get; set; }
    public DateTimeOffset? SubmittedAt { get; set; }

    public EmployeeSkill EmployeeSkill { get; set; } = null!;
    public ICollection<AssessmentResult> Results { get; set; } = new List<AssessmentResult>();
}

/// <summary>
/// Result of an evaluated assessment (score, pass/fail, comments).
/// </summary>
public class AssessmentResult : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid AssessmentId { get; set; }
    public decimal Score { get; set; }
    public bool? PassFail { get; set; }
    public string? Comments { get; set; }
    public DateTimeOffset? EvaluatedAt { get; set; }

    public Assessment Assessment { get; set; } = null!;
}

/// <summary>
/// In-app notification for an employee (pending approval, expiring cert, manager validated, etc.).
/// </summary>
public class Notification : AuditableEntity
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public NotificationType Type { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Body { get; set; }
    public DateTimeOffset? ReadAt { get; set; }
    public Guid? RelatedEntityId { get; set; }

    // No FK to Employee - Employee is from ITP identity module
}
