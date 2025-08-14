using Music.API.Interfaces;

namespace Music.API.MinimalApis;

public class UserEndpoints : IEndpoint
{
    public IEndpointRouteBuilder Register(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/v1");

        group.MapGet("users", () =>
        {
            return Results.Ok("users works");
        });
        group.MapGet("user", () => { });
        group.MapPost("users", () =>
        {
            return Results.Ok("User works");

        });
        group.MapPut("users", () =>
        {
            return Results.Ok();

        });

        group.DisableAntiforgery();

        return group;
    }
}
