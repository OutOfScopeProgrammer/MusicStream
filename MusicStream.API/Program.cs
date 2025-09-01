using Microsoft.AspNetCore.HttpOverrides;
using Music.API.Extensions;
using MusicStream.Application.Extensions;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Extensions;
using Prometheus;
using Scalar.AspNetCore;
var wwwrootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
if (!Directory.Exists(wwwrootPath))
{
    Directory.CreateDirectory(wwwrootPath);
}
var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(option =>
{
    option.Limits.MaxConcurrentConnections = 2000;
    option.Limits.MaxConcurrentUpgradedConnections = 2000; // for websockets/http2


});
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Configuration.AddEnvironmentVariables();
builder.Logging.ClearProviders().SetMinimumLevel(LogLevel.None);

builder.Services.AddApiLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddOpenApi();
builder.Services.AddOpenTelemetryConfiguration();
var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor |
    ForwardedHeaders.XForwardedHost |
    ForwardedHeaders.XForwardedProto
});
app.UseStaticFiles();
app.MapOpenApi();
if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
{
    options.WithTitle("Music Stream API")
           .WithTheme(ScalarTheme.Mars)
           .WithDarkMode();
});
}

app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var connection = scope.ServiceProvider.GetRequiredService<IBucketManager>();
var seeder = scope.ServiceProvider.GetRequiredService<ISeeder>();
await seeder.MigrateAsync();
await connection.MainBucketInitializer();

app.AddEndpoints();
app.MapMetrics();
app.MapControllers();
app.Run();
