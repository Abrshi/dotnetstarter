using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using starterBackend.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using starter.Application.Auth.Commands.Signup;
using starter.Application.Common.Behaviors;
using starter.Domain.Entities;
using starter.Infrastructure.Data;
var builder = WebApplication.CreateBuilder(args);

// --------------------------------------------------
// Controllers
// --------------------------------------------------

builder.Services.AddControllers();

// --------------------------------------------------
// EF Core
// --------------------------------------------------

builder.Services.AddDbContext<starterBackendDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("starterDatabase")
    )
);
// --------------------------------------------------
// MediatR
// --------------------------------------------------

builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(
        typeof(SignupCommand).Assembly);

    config.AddOpenBehavior(
        typeof(ValidationBehavior<,>));
});

// --------------------------------------------------
// API Versioning
// --------------------------------------------------

builder.Services
    .AddApiVersioning(options =>
    {
        options.DefaultApiVersion = new ApiVersion(1, 0);
        options.AssumeDefaultVersionWhenUnspecified = true;
        options.ReportApiVersions = true;
        options.ApiVersionReader = new UrlSegmentApiVersionReader();
    })
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
    });
// --------------------------------------------------
// FluentValidation
// --------------------------------------------------

builder.Services.AddValidatorsFromAssembly(
    typeof(SignupCommand).Assembly);

// --------------------------------------------------
// Password hashing
// --------------------------------------------------

builder.Services.AddScoped<
    IPasswordHasher<User>,
    PasswordHasher<User>>();

// --------------------------------------------------
// Build application
// --------------------------------------------------

var app = builder.Build();

// HTTP pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();