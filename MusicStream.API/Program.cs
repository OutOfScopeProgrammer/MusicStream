using Music.API.Extensions;
using MusicStream.Application.Extensions;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Extensions;
using Prometheus;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApiLayer(builder.Configuration);
builder.Services.AddInfrastructureLayer(builder.Configuration, builder.Environment);
builder.Services.AddApplicationLayer();
builder.Services.AddOpenApi();
builder.Environment.IsDevelopment();
builder.Services.AddOpenTelemetryConfiguration();
var app = builder.Build();
app.UseStaticFiles();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.WithTitle("Music Stream API")
           .WithTheme(ScalarTheme.Mars)
           .WithDarkMode();
});

app.UseAuthentication();
app.UseAuthorization();

using var scope = app.Services.CreateScope();
var connection = scope.ServiceProvider.GetRequiredService<IBucketManager>();
await connection.MainBucketInitializer();

app.AddEndpoints();
app.MapMetrics();
app.MapControllers();
app.Run();
