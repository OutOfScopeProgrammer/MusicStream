namespace Music.API.Api.Controllers.IdentityController;

public record class IdentityDto(string PhoneNumber, string Password);
public record IdentityResponse(string AccessToken);
public record class RefreshTokenDto(string RefreshToken);
