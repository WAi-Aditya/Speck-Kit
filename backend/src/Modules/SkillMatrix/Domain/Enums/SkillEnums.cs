namespace ITP.Api.Modules.SkillMatrix.Domain.Enums;

/// <summary>
/// Proficiency level for a skill (constitution P2).
/// </summary>
public enum SkillLevel
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4
}

/// <summary>
/// Manager validation status for an employee skill entry.
/// </summary>
public enum ValidationStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

/// <summary>
/// Type of assessment (self, manager, peer, system-generated).
/// </summary>
public enum AssessmentType
{
    Self = 0,
    Manager = 1,
    Peer = 2,
    System = 3
}

/// <summary>
/// Lifecycle status of an assessment.
/// </summary>
public enum AssessmentStatus
{
    Assigned = 0,
    Submitted = 1,
    Evaluated = 2
}

/// <summary>
/// Type of notification sent to the employee.
/// </summary>
public enum NotificationType
{
    PendingApproval = 0,
    ExpiringCertification = 1,
    OverdueAssessment = 2,
    ManagerValidated = 3,
    ManagerRejected = 4,
    AssessmentAssigned = 5
}
