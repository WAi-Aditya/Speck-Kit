namespace ITP.Api.Presentation.Swagger;

/// <summary>
/// Swagger/OpenAPI grouping and document configuration for ITP API.
/// Skill Matrix endpoints are grouped under <see cref="SkillMatrixGroupName"/>.
/// </summary>
public static class SwaggerConfig
{
    /// <summary>
    /// API group name for Skill Matrix module (taxonomy, profile, approval, certifications, assessments, reporting).
    /// Use this in controller [ApiExplorerSettings(GroupName = SwaggerConfig.SkillMatrixGroupName)] or when configuring SwaggerDoc.
    /// </summary>
    public const string SkillMatrixGroupName = "Skill Matrix";

    /// <summary>
    /// Description for the Skill Matrix API group shown in Swagger UI.
    /// </summary>
    public const string SkillMatrixGroupDescription = "Skill Matrix: taxonomy, employee profiles, manager approval, certifications, assessments, and reporting.";
}
