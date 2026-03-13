namespace ITP.Api.Modules.SkillMatrix.Application.Taxonomy;

public record CategoryDto(Guid Id, string Name, string? Description, bool IsActive, int SortOrder);
public record CreateCategoryRequest(string Name, string? Description, int SortOrder = 0);
public record UpdateCategoryRequest(string Name, string? Description, int SortOrder = 0);

public record SubCategoryDto(Guid Id, Guid SkillCategoryId, string Name, string? Description, bool IsActive, int SortOrder);
public record CreateSubCategoryRequest(Guid SkillCategoryId, string Name, string? Description, int SortOrder = 0);
public record UpdateSubCategoryRequest(string Name, string? Description, int SortOrder = 0);

public record SkillDto(Guid Id, Guid SkillSubCategoryId, string Name, string? Description, bool IsActive, int SortOrder);
public record CreateSkillRequest(Guid SkillSubCategoryId, string Name, string? Description, int SortOrder = 0);
public record UpdateSkillRequest(string Name, string? Description, int SortOrder = 0);

public record PagedResult<T>(IReadOnlyList<T> Items);
