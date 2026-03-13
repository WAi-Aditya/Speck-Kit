using ITP.Api.Infrastructure.Persistence;
using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using ITP.Api.Modules.SkillMatrix.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace ITP.Api.Modules.SkillMatrix.Application.Profile;

public interface IEmployeeSkillProfileService
{
    Task<MyProfileResponse?> GetMyProfileAsync(Guid employeeId, CancellationToken ct = default);
    Task<MyProfileResponse?> GetProfileAsync(Guid requestingEmployeeId, Guid targetEmployeeId, bool isManager, CancellationToken ct = default);
    Task<ProfileSkillDto?> AddSkillToProfileAsync(Guid employeeId, AddSkillToProfileRequest request, string? userId, CancellationToken ct = default);
    Task<bool> UpdateProfileSkillAsync(Guid employeeId, Guid employeeSkillId, UpdateProfileSkillRequest request, string? userId, CancellationToken ct = default);
}

public class EmployeeSkillProfileService : IEmployeeSkillProfileService
{
    private readonly ITPDbContext _db;

    public EmployeeSkillProfileService(ITPDbContext db) => _db = db;

    public async Task<MyProfileResponse?> GetMyProfileAsync(Guid employeeId, CancellationToken ct = default)
    {
        return await GetProfileInternalAsync(employeeId, ct);
    }

    public async Task<MyProfileResponse?> GetProfileAsync(Guid requestingEmployeeId, Guid targetEmployeeId, bool isManager, CancellationToken ct = default)
    {
        if (!isManager && requestingEmployeeId != targetEmployeeId)
            return null;
        return await GetProfileInternalAsync(targetEmployeeId, ct);
    }

    private async Task<MyProfileResponse?> GetProfileInternalAsync(Guid employeeId, CancellationToken ct)
    {
        var skills = await _db.EmployeeSkills
            .Where(x => x.EmployeeId == employeeId)
            .Include(x => x.Skill)
            .ThenInclude(s => s!.SkillSubCategory)
            .ThenInclude(sc => sc!.SkillCategory)
            .OrderBy(x => x.Skill!.SkillSubCategory!.SkillCategory!.Name)
            .ThenBy(x => x.Skill!.Name)
            .Select(x => new ProfileSkillDto(
                x.Id,
                x.SkillId,
                x.Skill!.Name,
                x.Skill.SkillSubCategory!.SkillCategory!.Name,
                x.Skill.SkillSubCategory.Name,
                x.SelfAssessedLevel,
                x.ManagerValidatedLevel,
                x.FinalRating,
                (int)x.ValidationStatus,
                x.ValidatedAt))
            .ToListAsync(ct);
        return new MyProfileResponse(employeeId, skills);
    }

    public async Task<ProfileSkillDto?> AddSkillToProfileAsync(Guid employeeId, AddSkillToProfileRequest request, string? userId, CancellationToken ct = default)
    {
        if (request.SelfAssessedLevel < 1 || request.SelfAssessedLevel > 4)
            throw new ArgumentException("SelfAssessedLevel must be between 1 and 4.", nameof(request));
        var skill = await _db.Skills
            .Include(s => s.SkillSubCategory)
            .ThenInclude(sc => sc!.SkillCategory)
            .FirstOrDefaultAsync(s => s.Id == request.SkillId && s.IsActive, ct);
        if (skill == null)
            throw new InvalidOperationException("Skill not found or inactive.");
        var exists = await _db.EmployeeSkills.AnyAsync(x => x.EmployeeId == employeeId && x.SkillId == request.SkillId, ct);
        if (exists)
            throw new InvalidOperationException("This skill is already on your profile.");
        var now = DateTimeOffset.UtcNow;
        var entity = new EmployeeSkill
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            SkillId = request.SkillId,
            SelfAssessedLevel = request.SelfAssessedLevel,
            ValidationStatus = ValidationStatus.Pending,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = userId
        };
        _db.EmployeeSkills.Add(entity);
        await _db.SaveChangesAsync(ct);
        return new ProfileSkillDto(
            entity.Id,
            entity.SkillId,
            skill.Name,
            skill.SkillSubCategory!.SkillCategory!.Name,
            skill.SkillSubCategory.Name,
            entity.SelfAssessedLevel,
            null,
            null,
            (int)entity.ValidationStatus,
            null);
    }

    public async Task<bool> UpdateProfileSkillAsync(Guid employeeId, Guid employeeSkillId, UpdateProfileSkillRequest request, string? userId, CancellationToken ct = default)
    {
        if (request.SelfAssessedLevel < 1 || request.SelfAssessedLevel > 4)
            throw new ArgumentException("SelfAssessedLevel must be between 1 and 4.", nameof(request));
        var entity = await _db.EmployeeSkills.FirstOrDefaultAsync(x => x.Id == employeeSkillId && x.EmployeeId == employeeId, ct);
        if (entity == null) return false;
        entity.SelfAssessedLevel = request.SelfAssessedLevel;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = userId;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
