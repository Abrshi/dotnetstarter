using Asp.Versioning;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using starter.Application.Auth.Commands.Signup;
using starter.Application.Common.Behaviors;
using starter.Application.Common.Interfaces;
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

builder.Services.AddDbContext<StarterDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("starterDatabase")
    )
);

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<StarterDbContext>());

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

// Auto-apply pending database migrations on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<StarterDbContext>();
    dbContext.Database.Migrate();
}

// HTTP pipeline
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();