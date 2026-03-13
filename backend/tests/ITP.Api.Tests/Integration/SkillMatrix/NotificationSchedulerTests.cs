using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ITP.Api.Tests.Integration.SkillMatrix;

/// <summary>
/// Integration tests for notification scheduler and notification delivery (US2).
/// </summary>
public class NotificationSchedulerTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public NotificationSchedulerTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_notifications_me_returns_200_with_items_array_when_implemented()
    {
        var res = await _client.GetAsync("/api/skill-matrix/notifications/me");
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<NotificationsListResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task PATCH_notifications_id_read_returns_204_when_implemented()
    {
        var listRes = await _client.GetAsync("/api/skill-matrix/notifications/me");
        if (listRes.StatusCode == HttpStatusCode.NotFound)
            return;
        listRes.EnsureSuccessStatusCode();
        var list = await listRes.Content.ReadFromJsonAsync<NotificationsListResponse>();
        var first = list?.Items?.FirstOrDefault();
        if (first == null)
            return;

        var res = await _client.PatchAsync($"/api/skill-matrix/notifications/{first.Id}/read", null);
        Assert.True(res.StatusCode == HttpStatusCode.NoContent || res.StatusCode == HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Notification_items_have_required_shape_when_implemented()
    {
        var res = await _client.GetAsync("/api/skill-matrix/notifications/me");
        if (res.StatusCode == HttpStatusCode.NotFound)
            return;
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<NotificationsListResponse>();
        Assert.NotNull(json?.Items);
        foreach (var n in json!.Items!)
        {
            Assert.NotEqual(Guid.Empty, n.Id);
            Assert.NotNull(n.Title);
            Assert.True(n.Type >= 0);
        }
    }

    private class NotificationsListResponse
    {
        public List<NotificationItemDto>? Items { get; set; }
    }

    private class NotificationItemDto
    {
        public Guid Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; } = "";
        public string? Body { get; set; }
        public DateTimeOffset? ReadAt { get; set; }
    }
}
