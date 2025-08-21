namespace Music.API.Api.Controllers.IdentityController;

public record class IdentityDto(string PhoneNumber, string Password);
public record IdentityResponse(string RefreshToken);
public record class RefreshTokenDto(string RefreshToken);


