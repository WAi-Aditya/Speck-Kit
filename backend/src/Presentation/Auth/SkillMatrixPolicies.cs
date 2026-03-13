using Microsoft.AspNetCore.Authorization;

namespace ITP.Api.Presentation.Auth;

/// <summary>
/// Role-based authorization policy names and extension for Skill Matrix module (constitution P4).
/// </summary>
public static class SkillMatrixPolicies
{
    public const string Employee = "SkillMatrix.Employee";
    public const string Manager = "SkillMatrix.Manager";
    public const string HRAdmin = "SkillMatrix.HRAdmin";
    public const string Leadership = "SkillMatrix.Leadership";

    /// <summary>
    /// Adds Skill Matrix authorization policies requiring the corresponding role claim (e.g. "role" or "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").
    /// </summary>
    public static AuthorizationOptions AddSkillMatrixPolicies(this AuthorizationOptions options)
    {
        options.AddPolicy(Employee, p => p.RequireRole("Employee"));
        options.AddPolicy(Manager, p => p.RequireRole("Employee", "Manager"));
        options.AddPolicy(HRAdmin, p => p.RequireRole("Employee", "Manager", "HRAdmin"));
        options.AddPolicy(Leadership, p => p.RequireRole("Employee", "Manager", "HRAdmin", "Leadership"));
        return options;
    }
}
