using Music.API.Extensions;
using MusicStream.Application.Extensions;
using MusicStream.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddInfrastructureLayer();
builder.Services.AddApplicationLayer();
builder.Services.AddApiLayer();
var app = builder.Build();

app.AddEndpoints();

app.Run();
