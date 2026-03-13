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

var app = builder.Build();
app.MapGet("/", () => Results.Ok(new { name = "ITP.Api", status = "running" }));
app.Run();
