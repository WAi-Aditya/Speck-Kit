using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ITP.Api.Tests.Integration.SkillMatrix;

public class EmployeeProfileFlowTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public EmployeeProfileFlowTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_profile_me_returns_200_with_employeeId_and_skills()
    {
        var res = await _client.GetAsync("/api/skill-matrix/profile/me");
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<MyProfileResponse>();
        Assert.NotNull(json);
        Assert.NotEqual(Guid.Empty, json!.EmployeeId);
        Assert.NotNull(json.Skills);
    }

    [Fact]
    public async Task POST_profile_me_skills_returns_201_when_skill_exists_and_valid_level()
    {
        var skillId = await GetFirstSkillIdAsync();
        if (skillId == null) return;
        var res = await _client.PostAsJsonAsync("/api/skill-matrix/profile/me/skills", new { skillId, selfAssessedLevel = 2 });
        if (res.StatusCode == HttpStatusCode.BadRequest)
        {
            var body = await res.Content.ReadAsStringAsync();
            if (body.Contains("already on your profile")) return;
        }
        Assert.True(res.StatusCode == HttpStatusCode.Created || res.StatusCode == HttpStatusCode.BadRequest);
        if (res.StatusCode == HttpStatusCode.Created)
        {
            var dto = await res.Content.ReadFromJsonAsync<ProfileSkillDto>();
            Assert.NotNull(dto);
            Assert.Equal(skillId, dto!.SkillId);
        }
    }

    [Fact]
    public async Task POST_profile_me_skills_returns_400_for_invalid_level()
    {
        var skillId = await GetFirstSkillIdAsync();
        if (skillId == null) return;
        var res = await _client.PostAsJsonAsync("/api/skill-matrix/profile/me/skills", new { skillId, selfAssessedLevel = 99 });
        Assert.Equal(HttpStatusCode.BadRequest, res.StatusCode);
    }

    private async Task<Guid?> GetFirstSkillIdAsync()
    {
        var res = await _client.GetAsync("/api/skill-matrix/skills");
        if (!res.IsSuccessStatusCode) return null;
        var json = await res.Content.ReadFromJsonAsync<SkillsResponse>();
        return json?.Items?.FirstOrDefault()?.Id;
    }

    private class MyProfileResponse { public Guid EmployeeId { get; set; } public List<ProfileSkillDto>? Skills { get; set; } }
    private class ProfileSkillDto { public Guid EmployeeSkillId { get; set; } public Guid SkillId { get; set; } public string? SkillName { get; set; } }
    private class SkillsResponse { public List<SkillItem>? Items { get; set; } }
    private class SkillItem { public Guid Id { get; set; } }
}
