namespace ITP.Api.Modules.SkillMatrix.Domain.Common;

/// <summary>
/// Base type for Skill Matrix entities that require audit fields (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy).
/// </summary>
public abstract class AuditableEntity
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? UpdatedBy { get; set; }
}
