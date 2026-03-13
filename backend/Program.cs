using ITP.Api.Infrastructure.Configuration;
using ITP.Api.Infrastructure.Persistence;
using ITP.Api.Presentation.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthorization(options => options.AddSkillMatrixPolicies());

builder.Services.Configure<SkillMatrixOptions>(
    builder.Configuration.GetSection(SkillMatrixOptions.SectionName));

builder.Services.AddDbContext<ITPDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=WAILAP205;Initial Catalog=ITP;Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True;TrustServerCertificate=True;",
        b => b.MigrationsAssembly("ITP.Api")));

builder.Services.AddControllers();

builder.Services.AddScoped<ITP.Api.Modules.SkillMatrix.Application.Taxonomy.ITaxonomyService, ITP.Api.Modules.SkillMatrix.Application.Taxonomy.TaxonomyService>();
builder.Services.AddScoped<ITP.Api.Modules.SkillMatrix.Application.Profile.IEmployeeSkillProfileService, ITP.Api.Modules.SkillMatrix.Application.Profile.EmployeeSkillProfileService>();
builder.Services.AddScoped<ITP.Api.Modules.SkillMatrix.Application.Approval.IManagerApprovalService, ITP.Api.Modules.SkillMatrix.Application.Approval.ManagerApprovalService>();

var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/", () => Results.Ok(new { name = "ITP.Api", status = "running" }));
app.MapControllers();
app.Run();
