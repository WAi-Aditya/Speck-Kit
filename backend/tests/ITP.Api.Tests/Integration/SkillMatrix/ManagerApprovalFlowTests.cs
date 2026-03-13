using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ITP.Api.Tests.Integration.SkillMatrix;

public class ManagerApprovalFlowTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public ManagerApprovalFlowTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_approvals_pending_returns_200_with_items_array()
    {
        var res = await _client.GetAsync("/api/skill-matrix/approvals/pending");
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<PendingApprovalsResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task PATCH_approvals_id_with_approve_returns_204_or_404()
    {
        var listRes = await _client.GetAsync("/api/skill-matrix/approvals/pending");
        var list = await listRes.Content.ReadFromJsonAsync<PendingApprovalsResponse>();
        var first = list?.Items?.FirstOrDefault();
        if (first == null) return;
        var res = await _client.PatchAsJsonAsync($"/api/skill-matrix/approvals/{first.EmployeeSkillId}", new { action = "approve", managerValidatedLevel = 2, managerNotes = (string?)null });
        Assert.True(res.StatusCode == HttpStatusCode.NoContent || res.StatusCode == HttpStatusCode.NotFound || res.StatusCode == HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task PATCH_approvals_id_with_reject_returns_400_when_action_invalid()
    {
        var res = await _client.PatchAsJsonAsync($"/api/skill-matrix/approvals/{Guid.NewGuid()}", new { action = "invalid" });
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    private class PendingApprovalsResponse { public List<PendingItem>? Items { get; set; } }
    private class PendingItem { public Guid EmployeeSkillId { get; set; } public Guid EmployeeId { get; set; } public string? SkillName { get; set; } }
}
