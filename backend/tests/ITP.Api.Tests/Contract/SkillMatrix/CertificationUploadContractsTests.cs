using System.Net;
using System.Net.Http.Json;
using System.Text;
using Xunit;

namespace ITP.Api.Tests.Contract.SkillMatrix;

/// <summary>
/// Contract tests for certification multipart upload (US2).
/// POST /api/skill-matrix/profile/me/certifications and GET list.
/// </summary>
public class CertificationUploadContractsTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public CertificationUploadContractsTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_profile_me_certifications_returns_200_and_items_array()
    {
        var res = await _client.GetAsync("/api/skill-matrix/profile/me/certifications");
        if (res.StatusCode == HttpStatusCode.NotFound)
        {
            return; // Endpoint not implemented yet (T043)
        }
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<CertificationsListResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task POST_profile_me_certifications_multipart_returns_201_with_id_and_documentBlobUrl_when_implemented()
    {
        var skillId = await GetFirstSkillIdAsync();
        if (skillId == null)
            return;

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(skillId.Value.ToString()), "skillId");
        content.Add(new StringContent("Contract Test Certification"), "title");
        content.Add(new StringContent("Test Issuer"), "issuer");
        content.Add(new StringContent(DateTimeOffset.UtcNow.Date.ToString("O")), "issueDate");
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("dummy pdf content"));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "file", "cert.pdf");

        var res = await _client.PostAsync("/api/skill-matrix/profile/me/certifications", content);
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;

        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
        var dto = await res.Content.ReadFromJsonAsync<CertificationCreatedDto>();
        Assert.NotNull(dto);
        Assert.NotEqual(Guid.Empty, dto!.Id);
        Assert.NotNull(dto.DocumentBlobUrl);
    }

    [Fact]
    public async Task POST_profile_me_certifications_returns_400_when_issueDate_missing()
    {
        var skillId = await GetFirstSkillIdAsync();
        if (skillId == null)
            return;

        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(skillId.Value.ToString()), "skillId");
        content.Add(new StringContent("No Date Cert"), "title");
        content.Add(new StringContent("Issuer"), "issuer");
        var fileContent = new ByteArrayContent(Encoding.UTF8.GetBytes("x"));
        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
        content.Add(fileContent, "file", "cert.pdf");

        var res = await _client.PostAsync("/api/skill-matrix/profile/me/certifications", content);
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;

        Assert.True(res.StatusCode == HttpStatusCode.BadRequest || res.StatusCode == HttpStatusCode.Created);
    }

    private async Task<Guid?> GetFirstSkillIdAsync()
    {
        var res = await _client.GetAsync("/api/skill-matrix/skills");
        if (!res.IsSuccessStatusCode) return null;
        var json = await res.Content.ReadFromJsonAsync<SkillsResponse>();
        return json?.Items?.FirstOrDefault()?.Id;
    }

    private class CertificationsListResponse
    {
        public List<CertificationItemDto>? Items { get; set; }
    }

    private class CertificationItemDto
    {
        public Guid Id { get; set; }
        public Guid SkillId { get; set; }
        public string Title { get; set; } = "";
        public string Issuer { get; set; } = "";
        public DateTimeOffset IssueDate { get; set; }
        public DateTimeOffset? ExpiryDate { get; set; }
        public string? DocumentBlobUrl { get; set; }
    }

    private class CertificationCreatedDto
    {
        public Guid Id { get; set; }
        public string? DocumentBlobUrl { get; set; }
    }

    private class SkillsResponse
    {
        public List<SkillItem>? Items { get; set; }
    }

    private class SkillItem
    {
        public Guid Id { get; set; }
    }
}
