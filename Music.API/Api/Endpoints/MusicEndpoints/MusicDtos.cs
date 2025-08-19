namespace Music.API.Api.Endpoints.MusicEndpoints;

public record CreateMusicDto(string Title, string Description, string SingerId, IFormFile File);
public record MusicDto(string Title, string Description, string StreamUrl);


