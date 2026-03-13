using ITP.Api.Presentation.Auth;
using ITP.Api.Presentation.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITP.Api.Modules.SkillMatrix.Presentation.Controllers;

[ApiController]
[Route("api/skill-matrix")]
[Authorize(Policy = SkillMatrixPolicies.HRAdmin)]
[ApiExplorerSettings(GroupName = SwaggerConfig.SkillMatrixGroupName)]
public class SkillTaxonomyController : ControllerBase
{
    private readonly Application.Taxonomy.ITaxonomyService _taxonomy;

    public SkillTaxonomyController(Application.Taxonomy.ITaxonomyService taxonomy) => _taxonomy = taxonomy;

    [HttpGet("categories")]
    [ProducesResponseType(typeof(Application.Taxonomy.PagedResult<Application.Taxonomy.CategoryDto>), 200)]
    public async Task<IActionResult> GetCategories(CancellationToken ct)
    {
        var result = await _taxonomy.GetCategoriesAsync(ct);
        return Ok(result);
    }

    [HttpGet("categories/{id:guid}")]
    [ProducesResponseType(typeof(Application.Taxonomy.CategoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetCategory(Guid id, CancellationToken ct)
    {
        var dto = await _taxonomy.GetCategoryByIdAsync(id, ct);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost("categories")]
    [ProducesResponseType(typeof(Application.Taxonomy.CategoryDto), 201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory([FromBody] Application.Taxonomy.CreateCategoryRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var dto = await _taxonomy.CreateCategoryAsync(request, userId, ct);
            return CreatedAtAction(nameof(GetCategory), new { id = dto.Id }, dto);
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpPut("categories/{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] Application.Taxonomy.UpdateCategoryRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var ok = await _taxonomy.UpdateCategoryAsync(id, request, userId, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpDelete("categories/{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    [ProducesResponseType(409)]
    public async Task<IActionResult> DeactivateCategory(Guid id, CancellationToken ct)
    {
        try
        {
            var ok = await _taxonomy.DeactivateCategoryAsync(id, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (InvalidOperationException ex) { return StatusCode(409, new { detail = ex.Message }); }
    }

    [HttpGet("subcategories")]
    [ProducesResponseType(typeof(Application.Taxonomy.PagedResult<Application.Taxonomy.SubCategoryDto>), 200)]
    public async Task<IActionResult> GetSubCategories([FromQuery] Guid? categoryId, CancellationToken ct)
    {
        var result = await _taxonomy.GetSubCategoriesAsync(categoryId, ct);
        return Ok(result);
    }

    [HttpGet("subcategories/{id:guid}")]
    [ProducesResponseType(typeof(Application.Taxonomy.SubCategoryDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetSubCategory(Guid id, CancellationToken ct)
    {
        var dto = await _taxonomy.GetSubCategoryByIdAsync(id, ct);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost("subcategories")]
    [ProducesResponseType(typeof(Application.Taxonomy.SubCategoryDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateSubCategory([FromBody] Application.Taxonomy.CreateSubCategoryRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var dto = await _taxonomy.CreateSubCategoryAsync(request, userId, ct);
            return CreatedAtAction(nameof(GetSubCategory), new { id = dto.Id }, dto);
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return ex.Message.Contains("category") ? NotFound(new { detail = ex.Message }) : BadRequest(new { detail = ex.Message }); }
    }

    [HttpPut("subcategories/{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateSubCategory(Guid id, [FromBody] Application.Taxonomy.UpdateSubCategoryRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var ok = await _taxonomy.UpdateSubCategoryAsync(id, request, userId, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpGet("skills")]
    [ProducesResponseType(typeof(Application.Taxonomy.PagedResult<Application.Taxonomy.SkillDto>), 200)]
    public async Task<IActionResult> GetSkills([FromQuery] Guid? subcategoryId, CancellationToken ct)
    {
        var result = await _taxonomy.GetSkillsAsync(subcategoryId, ct);
        return Ok(result);
    }

    [HttpGet("skills/{id:guid}")]
    [ProducesResponseType(typeof(Application.Taxonomy.SkillDto), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetSkill(Guid id, CancellationToken ct)
    {
        var dto = await _taxonomy.GetSkillByIdAsync(id, ct);
        if (dto == null) return NotFound();
        return Ok(dto);
    }

    [HttpPost("skills")]
    [ProducesResponseType(typeof(Application.Taxonomy.SkillDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> CreateSkill([FromBody] Application.Taxonomy.CreateSkillRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var dto = await _taxonomy.CreateSkillAsync(request, userId, ct);
            return CreatedAtAction(nameof(GetSkill), new { id = dto.Id }, dto);
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return ex.Message.Contains("subcategory") ? NotFound(new { detail = ex.Message }) : BadRequest(new { detail = ex.Message }); }
    }

    [HttpPut("skills/{id:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateSkill(Guid id, [FromBody] Application.Taxonomy.UpdateSkillRequest request, CancellationToken ct)
    {
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var ok = await _taxonomy.UpdateSkillAsync(id, request, userId, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }
}
