using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace ITP.Api.Tests.Contract.SkillMatrix;

public class TaxonomyContractsTests : IClassFixture<WebAppFactory>
{
    private readonly HttpClient _client;

    public TaxonomyContractsTests(WebAppFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GET_categories_returns_200_and_items_array()
    {
        var res = await _client.GetAsync("/api/skill-matrix/categories");
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<CategoriesResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task GET_categories_id_returns_200_when_exists()
    {
        var listRes = await _client.GetAsync("/api/skill-matrix/categories");
        var list = await listRes.Content.ReadFromJsonAsync<CategoriesResponse>();
        if (list?.Items == null || list.Items.Count == 0)
        {
            await CreateCategoryAndGetId();
            listRes = await _client.GetAsync("/api/skill-matrix/categories");
            list = await listRes.Content.ReadFromJsonAsync<CategoriesResponse>();
        }
        Assert.NotNull(list?.Items);
        var id = list.Items[0].Id;
        var res = await _client.GetAsync($"/api/skill-matrix/categories/{id}");
        res.EnsureSuccessStatusCode();
        var cat = await res.Content.ReadFromJsonAsync<CategoryDto>();
        Assert.NotNull(cat);
        Assert.Equal(id, cat.Id);
        Assert.NotNull(cat.Name);
    }

    [Fact]
    public async Task POST_categories_returns_201_and_created_category()
    {
        var res = await _client.PostAsJsonAsync("/api/skill-matrix/categories", new { name = "Contract Test Category " + Guid.NewGuid().ToString("N")[..8], description = (string?)null, sortOrder = 0 });
        Assert.Equal(HttpStatusCode.Created, res.StatusCode);
        var cat = await res.Content.ReadFromJsonAsync<CategoryDto>();
        Assert.NotNull(cat);
        Assert.NotEqual(Guid.Empty, cat.Id);
        Assert.NotNull(cat.Name);
    }

    [Fact]
    public async Task GET_subcategories_returns_200_and_items_array()
    {
        var res = await _client.GetAsync("/api/skill-matrix/subcategories");
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<SubCategoriesResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    [Fact]
    public async Task GET_skills_returns_200_and_items_array()
    {
        var res = await _client.GetAsync("/api/skill-matrix/skills");
        res.EnsureSuccessStatusCode();
        var json = await res.Content.ReadFromJsonAsync<SkillsResponse>();
        Assert.NotNull(json);
        Assert.NotNull(json!.Items);
    }

    private async Task<Guid> CreateCategoryAndGetId()
    {
        var res = await _client.PostAsJsonAsync("/api/skill-matrix/categories", new { name = "Test " + Guid.NewGuid().ToString("N")[..8], sortOrder = 0 });
        res.EnsureSuccessStatusCode();
        var cat = await res.Content.ReadFromJsonAsync<CategoryDto>();
        return cat!.Id;
    }

    private class CategoriesResponse { public List<CategoryDto>? Items { get; set; } }
    private class SubCategoriesResponse { public List<SubCategoryDto>? Items { get; set; } }
    private class SkillsResponse { public List<SkillDto>? Items { get; set; } }
    private class CategoryDto { public Guid Id { get; set; } public string Name { get; set; } = ""; public string? Description { get; set; } public bool IsActive { get; set; } public int SortOrder { get; set; } }
    private class SubCategoryDto { public Guid Id { get; set; } public Guid SkillCategoryId { get; set; } public string Name { get; set; } = ""; }
    private class SkillDto { public Guid Id { get; set; } public Guid SkillSubCategoryId { get; set; } public string Name { get; set; } = ""; }
}
