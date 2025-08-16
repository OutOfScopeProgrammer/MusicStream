using Music.API.Extensions;
using MusicStream.Application.Extensions;
using MusicStream.Application.Interfaces;
using MusicStream.Infrastructure.Extensions;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureLayer(builder.Configuration);
builder.Services.AddApplicationLayer();
builder.Services.AddApiLayer();
builder.Services.AddOpenApi();
var app = builder.Build();
app.MapOpenApi();
app.MapScalarApiReference();
app.AddEndpoints();
using var scope = app.Services.CreateScope();
var connection = scope.ServiceProvider.GetRequiredService<IBucketManager>();
await connection.MainBucketInitializer();
app.Run();
