namespace ITP.Api.Modules.SkillMatrix.Application.Approval;

public record PendingApprovalItemDto(
    Guid EmployeeSkillId,
    Guid EmployeeId,
    string EmployeeName,
    string SkillName,
    string CategoryName,
    string SubCategoryName,
    int? SelfAssessedLevel,
    DateTimeOffset SubmittedAt);

public record PendingApprovalsResponse(IReadOnlyList<PendingApprovalItemDto> Items);

public record ApprovalActionRequest(string Action, int? ManagerValidatedLevel, string? ManagerNotes); // Action: "approve" | "reject"
