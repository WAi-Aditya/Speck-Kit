using ITP.Api.Presentation.Auth;
using ITP.Api.Presentation.Swagger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ITP.Api.Modules.SkillMatrix.Presentation.Controllers;

[ApiController]
[Route("api/skill-matrix")]
[Authorize(Policy = SkillMatrixPolicies.Manager)]
[ApiExplorerSettings(GroupName = SwaggerConfig.SkillMatrixGroupName)]
public class ManagerApprovalController : ControllerBase
{
    private readonly Application.Approval.IManagerApprovalService _approval;

    public ManagerApprovalController(Application.Approval.IManagerApprovalService approval) => _approval = approval;

    [HttpGet("approvals/pending")]
    [ProducesResponseType(typeof(Application.Approval.PendingApprovalsResponse), 200)]
    public async Task<IActionResult> GetPendingApprovals(CancellationToken ct)
    {
        var managerEmployeeId = GetCurrentEmployeeId();
        if (managerEmployeeId == null) return Unauthorized();
        var result = await _approval.GetPendingApprovalsAsync(managerEmployeeId.Value, ct);
        return Ok(result);
    }

    [HttpPatch("approvals/{employeeSkillId:guid}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> ApproveOrReject(Guid employeeSkillId, [FromBody] Application.Approval.ApprovalActionRequest request, CancellationToken ct)
    {
        var managerEmployeeId = GetCurrentEmployeeId();
        if (managerEmployeeId == null) return Unauthorized();
        try
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier) ?? User.Identity?.Name;
            var ok = await _approval.ApproveOrRejectAsync(managerEmployeeId.Value, employeeSkillId, request, userId, ct);
            if (!ok) return NotFound();
            return NoContent();
        }
        catch (ArgumentException ex) { return BadRequest(new { detail = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { detail = ex.Message }); }
    }

    private Guid? GetCurrentEmployeeId()
    {
        var sub = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(sub) || !Guid.TryParse(sub, out var id)) return null;
        return id;
    }
}
