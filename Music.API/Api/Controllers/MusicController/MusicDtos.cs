namespace Music.API.Api.Controllers.MusicController;

public record CreateMusicDto(string Title, string Description, string SingerId, IFormFile File);
public record MusicDto(string Title, string Description, string StreamUrl);


