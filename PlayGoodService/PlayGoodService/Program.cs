using PlayGoodService.Data;
using PlayGoodService.Repositories;
using PlayGoodService.Services;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using PlayGoodService.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using PlayGoodBriefingService.Repositories;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = new JwtSettings();
builder.Configuration.Bind("JwtSettings", jwtSettings);

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

ConfigureJwtAuthentication(builder, jwtSettings);


builder.Services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Program>())
    .AddJsonOptions(x =>
        x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);

builder.Services.AddDbContext<AssetAppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));

builder.Services.AddScoped<IAssetRepository, AssetRepository>();
builder.Services.AddScoped<IAssetService, AssetService>();
builder.Services.AddScoped<IBriefingRepository, BriefingRepository>();
builder.Services.AddScoped<IBriefingService, BriefingService>();
builder.Services.AddScoped<IContentDistributionRepository, ContentDistributionRepository>();
builder.Services.AddScoped<IContentDistributionService, ContentDistributionService>();

builder.Services.AddHealthChecks();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AssetAppDbContext>();
    DbInitializer.Initialize(context);
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/health");

app.Run();

void ConfigureJwtAuthentication(WebApplicationBuilder builder, JwtSettings jwtSettings)
{
    var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtSettings.Audience,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuerSigningKey = true,
        };
    });
}
