using ITP.Api.Infrastructure.Persistence;
using ITP.Api.Modules.SkillMatrix.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ITP.Api.Modules.SkillMatrix.Application.Taxonomy;

public interface ITaxonomyService
{
    Task<PagedResult<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default);
    Task<CategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken ct = default);
    Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, string? userId, CancellationToken ct = default);
    Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request, string? userId, CancellationToken ct = default);
    Task<bool> DeactivateCategoryAsync(Guid id, CancellationToken ct = default);

    Task<PagedResult<SubCategoryDto>> GetSubCategoriesAsync(Guid? categoryId, CancellationToken ct = default);
    Task<SubCategoryDto?> GetSubCategoryByIdAsync(Guid id, CancellationToken ct = default);
    Task<SubCategoryDto> CreateSubCategoryAsync(CreateSubCategoryRequest request, string? userId, CancellationToken ct = default);
    Task<bool> UpdateSubCategoryAsync(Guid id, UpdateSubCategoryRequest request, string? userId, CancellationToken ct = default);

    Task<PagedResult<SkillDto>> GetSkillsAsync(Guid? subcategoryId, CancellationToken ct = default);
    Task<SkillDto?> GetSkillByIdAsync(Guid id, CancellationToken ct = default);
    Task<SkillDto> CreateSkillAsync(CreateSkillRequest request, string? userId, CancellationToken ct = default);
    Task<bool> UpdateSkillAsync(Guid id, UpdateSkillRequest request, string? userId, CancellationToken ct = default);
}

public class TaxonomyService : ITaxonomyService
{
    private readonly ITPDbContext _db;

    public TaxonomyService(ITPDbContext db) => _db = db;

    public async Task<PagedResult<CategoryDto>> GetCategoriesAsync(CancellationToken ct = default)
    {
        var list = await _db.SkillCategories
            .OrderBy(x => x.SortOrder).ThenBy(x => x.Name)
            .Select(x => new CategoryDto(x.Id, x.Name, x.Description, x.IsActive, x.SortOrder))
            .ToListAsync(ct);
        return new PagedResult<CategoryDto>(list);
    }

    public async Task<CategoryDto?> GetCategoryByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _db.SkillCategories.FindAsync([id], ct);
        return e == null ? null : new CategoryDto(e.Id, e.Name, e.Description, e.IsActive, e.SortOrder);
    }

    public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var exists = await _db.SkillCategories.AnyAsync(x => x.Name == request.Name, ct);
        if (exists) throw new InvalidOperationException("A category with this name already exists.");
        var now = DateTimeOffset.UtcNow;
        var entity = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null,
            IsActive = true,
            SortOrder = request.SortOrder,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = userId
        };
        _db.SkillCategories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return new CategoryDto(entity.Id, entity.Name, entity.Description, entity.IsActive, entity.SortOrder);
    }

    public async Task<bool> UpdateCategoryAsync(Guid id, UpdateCategoryRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var entity = await _db.SkillCategories.FindAsync([id], ct);
        if (entity == null) return false;
        var duplicate = await _db.SkillCategories.AnyAsync(x => x.Name == request.Name.Trim() && x.Id != id, ct);
        if (duplicate) throw new InvalidOperationException("A category with this name already exists.");
        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null;
        entity.SortOrder = request.SortOrder;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = userId;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeactivateCategoryAsync(Guid id, CancellationToken ct = default)
    {
        var entity = await _db.SkillCategories.FindAsync([id], ct);
        if (entity == null) return false;
        var inUse = await _db.SkillSubCategories.AnyAsync(x => x.SkillCategoryId == id, ct);
        if (inUse) throw new InvalidOperationException("Category is in use by subcategories; cannot deactivate.");
        entity.IsActive = false;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PagedResult<SubCategoryDto>> GetSubCategoriesAsync(Guid? categoryId, CancellationToken ct = default)
    {
        var query = _db.SkillSubCategories.Include(x => x.SkillCategory).AsQueryable();
        if (categoryId.HasValue)
            query = query.Where(x => x.SkillCategoryId == categoryId.Value);
        var list = await query
            .OrderBy(x => x.SortOrder).ThenBy(x => x.Name)
            .Select(x => new SubCategoryDto(x.Id, x.SkillCategoryId, x.Name, x.Description, x.IsActive, x.SortOrder))
            .ToListAsync(ct);
        return new PagedResult<SubCategoryDto>(list);
    }

    public async Task<SubCategoryDto?> GetSubCategoryByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _db.SkillSubCategories.FindAsync([id], ct);
        return e == null ? null : new SubCategoryDto(e.Id, e.SkillCategoryId, e.Name, e.Description, e.IsActive, e.SortOrder);
    }

    public async Task<SubCategoryDto> CreateSubCategoryAsync(CreateSubCategoryRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var category = await _db.SkillCategories.FindAsync([request.SkillCategoryId], ct);
        if (category == null || !category.IsActive)
            throw new InvalidOperationException("Invalid or inactive category.");
        var exists = await _db.SkillSubCategories.AnyAsync(x => x.SkillCategoryId == request.SkillCategoryId && x.Name == request.Name, ct);
        if (exists) throw new InvalidOperationException("A subcategory with this name already exists in this category.");
        var now = DateTimeOffset.UtcNow;
        var entity = new SkillSubCategory
        {
            Id = Guid.NewGuid(),
            SkillCategoryId = request.SkillCategoryId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null,
            IsActive = true,
            SortOrder = request.SortOrder,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = userId
        };
        _db.SkillSubCategories.Add(entity);
        await _db.SaveChangesAsync(ct);
        return new SubCategoryDto(entity.Id, entity.SkillCategoryId, entity.Name, entity.Description, entity.IsActive, entity.SortOrder);
    }

    public async Task<bool> UpdateSubCategoryAsync(Guid id, UpdateSubCategoryRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var entity = await _db.SkillSubCategories.FindAsync([id], ct);
        if (entity == null) return false;
        var duplicate = await _db.SkillSubCategories.AnyAsync(x => x.SkillCategoryId == entity.SkillCategoryId && x.Name == request.Name.Trim() && x.Id != id, ct);
        if (duplicate) throw new InvalidOperationException("A subcategory with this name already exists in this category.");
        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null;
        entity.SortOrder = request.SortOrder;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = userId;
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<PagedResult<SkillDto>> GetSkillsAsync(Guid? subcategoryId, CancellationToken ct = default)
    {
        var query = _db.Skills.AsQueryable();
        if (subcategoryId.HasValue)
            query = query.Where(x => x.SkillSubCategoryId == subcategoryId.Value);
        var list = await query
            .OrderBy(x => x.SortOrder).ThenBy(x => x.Name)
            .Select(x => new SkillDto(x.Id, x.SkillSubCategoryId, x.Name, x.Description, x.IsActive, x.SortOrder))
            .ToListAsync(ct);
        return new PagedResult<SkillDto>(list);
    }

    public async Task<SkillDto?> GetSkillByIdAsync(Guid id, CancellationToken ct = default)
    {
        var e = await _db.Skills.FindAsync([id], ct);
        return e == null ? null : new SkillDto(e.Id, e.SkillSubCategoryId, e.Name, e.Description, e.IsActive, e.SortOrder);
    }

    public async Task<SkillDto> CreateSkillAsync(CreateSkillRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var sub = await _db.SkillSubCategories.FindAsync([request.SkillSubCategoryId], ct);
        if (sub == null || !sub.IsActive)
            throw new InvalidOperationException("Invalid or inactive subcategory.");
        var exists = await _db.Skills.AnyAsync(x => x.SkillSubCategoryId == request.SkillSubCategoryId && x.Name == request.Name, ct);
        if (exists) throw new InvalidOperationException("A skill with this name already exists in this subcategory.");
        var now = DateTimeOffset.UtcNow;
        var entity = new Skill
        {
            Id = Guid.NewGuid(),
            SkillSubCategoryId = request.SkillSubCategoryId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null,
            IsActive = true,
            SortOrder = request.SortOrder,
            CreatedAt = now,
            UpdatedAt = now,
            CreatedBy = userId
        };
        _db.Skills.Add(entity);
        await _db.SaveChangesAsync(ct);
        return new SkillDto(entity.Id, entity.SkillSubCategoryId, entity.Name, entity.Description, entity.IsActive, entity.SortOrder);
    }

    public async Task<bool> UpdateSkillAsync(Guid id, UpdateSkillRequest request, string? userId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name) || request.Name.Length > 200)
            throw new ArgumentException("Name is required and max 200 characters.", nameof(request));
        var entity = await _db.Skills.FindAsync([id], ct);
        if (entity == null) return false;
        var duplicate = await _db.Skills.AnyAsync(x => x.SkillSubCategoryId == entity.SkillSubCategoryId && x.Name == request.Name.Trim() && x.Id != id, ct);
        if (duplicate) throw new InvalidOperationException("A skill with this name already exists in this subcategory.");
        entity.Name = request.Name.Trim();
        entity.Description = request.Description?.Trim().Length > 0 ? request.Description.Trim() : null;
        entity.SortOrder = request.SortOrder;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = userId;
        await _db.SaveChangesAsync(ct);
        return true;
    }
}
