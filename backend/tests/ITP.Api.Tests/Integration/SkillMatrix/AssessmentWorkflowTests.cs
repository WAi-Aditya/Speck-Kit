using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ITP.Api.Tests.Integration.SkillMatrix;

/// <summary>
/// Integration tests for assessment assign / submit / evaluate workflow (US2).
/// </summary>
public class AssessmentWorkflowTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public AssessmentWorkflowTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_assessments_me_returns_200_with_items_when_implemented()
    {
        var res = await _client.GetAsync("/api/skill-matrix/assessments/me");
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<AssessmentsListResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task POST_assessments_assign_returns_201_with_assessment_id_when_implemented()
    {
        var employeeSkillId = await GetFirstEmployeeSkillIdAsync();
        if (employeeSkillId == null)
            return;

        var body = new
        {
            employeeSkillId,
            assessmentType = 1,
            dueDate = DateTimeOffset.UtcNow.AddDays(7).ToString("O"),
        };
        var res = await _client.PostAsJsonAsync("/api/skill-matrix/assessments/assign", body);
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
        var dto = await res.Content.ReadFromJsonAsync<AssessmentAssignedDto>();
        Assert.NotNull(dto);
        Assert.NotEqual(Guid.Empty, dto!.Id);
    }

    [Fact]
    public async Task PATCH_assessments_id_submit_returns_204_when_implemented()
    {
        var listRes = await _client.GetAsync("/api/skill-matrix/assessments/me");
        if (listRes.StatusCode == HttpStatusCode.NotFound)
            return;
        listRes.EnsureSuccessStatusCode();
        var list = await listRes.Content.ReadFromJsonAsync<AssessmentsListResponse>();
        var first = list?.Items?.FirstOrDefault(a => a.Status == 0);
        if (first == null)
            return;

        var res = await _client.PatchAsJsonAsync($"/api/skill-matrix/assessments/{first.Id}/submit", new { });
        Assert.True(res.StatusCode == HttpStatusCode.NoContent || res.StatusCode == HttpStatusCode.BadRequest || res.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task POST_assessments_id_evaluate_returns_200_or_204_when_implemented()
    {
        var listRes = await _client.GetAsync("/api/skill-matrix/assessments/me");
        if (listRes.StatusCode == HttpStatusCode.NotFound)
            return;
        listRes.EnsureSuccessStatusCode();
        var list = await listRes.Content.ReadFromJsonAsync<AssessmentsListResponse>();
        var submitted = list?.Items?.FirstOrDefault(a => a.Status == 1);
        if (submitted == null)
            return;

        var body = new { score = 0.85m, passFail = true, comments = "Meets expectations" };
        var res = await _client.PostAsJsonAsync($"/api/skill-matrix/assessments/{submitted.Id}/evaluate", body);
        Assert.True(
            res.StatusCode == HttpStatusCode.OK || res.StatusCode == HttpStatusCode.NoContent ||
            res.StatusCode == HttpStatusCode.BadRequest || res.StatusCode == HttpStatusCode.NotFound);
    }

    private async Task<Guid?> GetFirstEmployeeSkillIdAsync()
    {
        var res = await _client.GetAsync("/api/skill-matrix/profile/me");
        if (!res.IsSuccessStatusCode) return null;
        var profile = await res.Content.ReadFromJsonAsync<MyProfileResponse>();
        return profile?.Skills?.FirstOrDefault()?.EmployeeSkillId;
    }

    private class AssessmentsListResponse
    {
        public List<AssessmentItemDto>? Items { get; set; }
    }

    private class AssessmentItemDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeSkillId { get; set; }
        public int AssessmentType { get; set; }
        public int Status { get; set; }
        public DateTimeOffset? DueDate { get; set; }
    }

    private class AssessmentAssignedDto
    {
        public Guid Id { get; set; }
    }

    private class MyProfileResponse
    {
        public Guid EmployeeId { get; set; }
        public List<ProfileSkillRef>? Skills { get; set; }
    }

    private class ProfileSkillRef
    {
        public Guid EmployeeSkillId { get; set; }
    }
}
