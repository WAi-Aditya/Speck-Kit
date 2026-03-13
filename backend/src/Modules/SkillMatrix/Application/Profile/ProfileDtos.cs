namespace ITP.Api.Modules.SkillMatrix.Application.Profile;

public record ProfileSkillDto(
    Guid EmployeeSkillId,
    Guid SkillId,
    string SkillName,
    string CategoryName,
    string SubCategoryName,
    int? SelfAssessedLevel,
    int? ManagerValidatedLevel,
    decimal? FinalRating,
    int ValidationStatus, // 0 Pending, 1 Approved, 2 Rejected
    DateTimeOffset? ValidatedAt);

public record MyProfileResponse(Guid EmployeeId, IReadOnlyList<ProfileSkillDto> Skills);

public record AddSkillToProfileRequest(Guid SkillId, int SelfAssessedLevel); // 1-4
public record UpdateProfileSkillRequest(int SelfAssessedLevel);
