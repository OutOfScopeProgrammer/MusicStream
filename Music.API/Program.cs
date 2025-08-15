using Music.API.Extensions;
using MusicStream.Application.Extensions;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddApiLayer();
var app = builder.Build();

app.AddEndpoints();
using var scope = app.Services.CreateScope();
var connection = scope.ServiceProvider.GetRequiredService<IBucketManager>();
await connection.MainBucketInitializer();
app.Run();
