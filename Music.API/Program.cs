using MusicStream.Application.Extensions;
using MusicStream.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
