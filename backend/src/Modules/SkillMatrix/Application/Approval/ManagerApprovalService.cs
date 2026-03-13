using ITP.Api.Infrastructure.Persistence;
using ITP.Api.Modules.SkillMatrix.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITP.Api.Modules.SkillMatrix.Application.Approval;

public interface IManagerApprovalService
{
    Task<PendingApprovalsResponse> GetPendingApprovalsAsync(Guid managerEmployeeId, CancellationToken ct = default);
    Task<bool> ApproveOrRejectAsync(Guid managerEmployeeId, Guid employeeSkillId, ApprovalActionRequest request, string? userId, CancellationToken ct = default);
}

public class ManagerApprovalService : IManagerApprovalService
{
    private readonly ITPDbContext _db;

    public ManagerApprovalService(ITPDbContext db) => _db = db;

    public async Task<PendingApprovalsResponse> GetPendingApprovalsAsync(Guid managerEmployeeId, CancellationToken ct = default)
    {
        // In a full implementation we would filter by direct reports; for MVP we return all pending for the org
        var items = await _db.EmployeeSkills
            .Where(x => x.ValidationStatus == ValidationStatus.Pending)
            .Include(x => x.Skill)
            .ThenInclude(s => s!.SkillSubCategory)
            .ThenInclude(sc => sc!.SkillCategory)
            .OrderBy(x => x.UpdatedAt)
            .Select(x => new PendingApprovalItemDto(
                x.Id,
                x.EmployeeId,
                "", // EmployeeName would come from identity/HR service
                x.Skill!.Name,
                x.Skill.SkillSubCategory!.SkillCategory!.Name,
                x.Skill.SkillSubCategory.Name,
                x.SelfAssessedLevel,
                x.UpdatedAt))
            .ToListAsync(ct);
        return new PendingApprovalsResponse(items);
    }

    public async Task<bool> ApproveOrRejectAsync(Guid managerEmployeeId, Guid employeeSkillId, ApprovalActionRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Action) || !new[] { "approve", "reject" }.Contains(request.Action.ToLowerInvariant()))
            throw new ArgumentException("Action must be 'approve' or 'reject'.", nameof(request));

        var entity = await _db.EmployeeSkills.FindAsync([employeeSkillId], ct);
        if (entity == null) return false;
        if (entity.ValidationStatus != ValidationStatus.Pending)
            throw new InvalidOperationException("This item is no longer pending approval.");

        var now = DateTimeOffset.UtcNow;
        if (request.Action.Equals("reject", StringComparison.OrdinalIgnoreCase))
        {
            entity.ValidationStatus = ValidationStatus.Rejected;
            entity.ManagerNotes = request.ManagerNotes?.Length > 2000 ? request.ManagerNotes[..2000] : request.ManagerNotes;
            entity.ValidatedAt = now;
            entity.ValidatedBy = managerEmployeeId;
            entity.UpdatedAt = now;
            entity.UpdatedBy = userId;
            await _db.SaveChangesAsync(ct);
            return true;
        }

        // approve
        int level = request.ManagerValidatedLevel ?? entity.SelfAssessedLevel ?? 1;
        if (level < 1 || level > 4)
            throw new ArgumentException("ManagerValidatedLevel must be between 1 and 4.", nameof(request));
        entity.ManagerValidatedLevel = level;
        entity.ValidationStatus = ValidationStatus.Approved;
        entity.ManagerNotes = request.ManagerNotes?.Length > 2000 ? request.ManagerNotes[..2000] : request.ManagerNotes;
        entity.ValidatedAt = now;
        entity.ValidatedBy = managerEmployeeId;
        entity.UpdatedAt = now;
        entity.UpdatedBy = userId;
        // FinalRating: when we have system score we'd do 0.4*Self + 0.4*Manager + 0.2*System; for MVP use (Self+Manager)/2 normalized or just manager
        entity.FinalRating = (entity.SelfAssessedLevel.HasValue ? (entity.SelfAssessedLevel.Value + level) / 2m / 4m : level / 4m);
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
