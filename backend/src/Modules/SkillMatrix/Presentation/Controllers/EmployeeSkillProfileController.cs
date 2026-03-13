using ITP.Api.Presentation.Auth;
using ITP.Api.Presentation.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITP.Api.Modules.SkillMatrix.Presentation.Controllers;

[ApiController]
[Route("api/skill-matrix")]
[Authorize(Policy = SkillMatrixPolicies.Employee)]
[ApiExplorerSettings(GroupName = SwaggerConfig.SkillMatrixGroupName)]
public class EmployeeSkillProfileController : ControllerBase
{
    private readonly Application.Profile.IEmployeeSkillProfileService _profile;

    public EmployeeSkillProfileController(Application.Profile.IEmployeeSkillProfileService profile) => _profile = profile;

    [HttpGet("profile/me")]
    [ProducesResponseType(typeof(Application.Profile.MyProfileResponse), 200)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetMyProfile(CancellationToken ct)
    {
        var employeeId = GetCurrentEmployeeId();
        if (employeeId == null) return Unauthorized();
        var result = await _profile.GetMyProfileAsync(employeeId.Value, ct);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpPost("profile/me/skills")]
    [ProducesResponseType(typeof(Application.Profile.ProfileSkillDto), 201)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> AddSkillToProfile([FromBody] Application.Profile.AddSkillToProfileRequest request, CancellationToken ct)
    {
        var employeeId = GetCurrentEmployeeId();
        if (employeeId == null) return Unauthorized();
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var dto = await _profile.AddSkillToProfileAsync(employeeId.Value, request, userId, ct);
            if (dto == null) return NotFound();
            return StatusCode(201, dto);
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpPatch("profile/me/skills/{employeeSkillId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateProfileSkill(Guid employeeSkillId, [FromBody] Application.Profile.UpdateProfileSkillRequest request, CancellationToken ct)
    {
        var employeeId = GetCurrentEmployeeId();
        if (employeeId == null) return Unauthorized();
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var ok = await _profile.UpdateProfileSkillAsync(employeeId.Value, employeeSkillId, request, userId, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    [HttpGet("profile/{employeeId:guid}")]
    [Authorize(Policy = SkillMatrixPolicies.Manager)]
    [ProducesResponseType(typeof(Application.Profile.MyProfileResponse), 200)]
    [ProducesResponseType(403)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> GetProfile(Guid employeeId, CancellationToken ct)
    {
        var currentId = GetCurrentEmployeeId();
        if (currentId == null) return Unauthorized();
        var isManager = User.IsInRole("Manager") || User.IsInRole("HRAdmin") || User.IsInRole("Leadership");
        var result = await _profile.GetProfileAsync(currentId.Value, employeeId, isManager, ct);
        if (result == null) return currentId != employeeId && !isManager ? Forbid() : NotFound();
        return Ok(result);
    }

    private Guid? GetCurrentEmployeeId()
    {
        // In real app: map from JWT sub/claim to EmployeeId via identity service
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var id)) return null;
        return id;
    }
}
