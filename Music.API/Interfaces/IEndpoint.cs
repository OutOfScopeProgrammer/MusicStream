namespace Music.API.Interfaces;

public interface IEndpoint
{
    IEndpointRouteBuilder Register(IEndpointRouteBuilder app);
}
