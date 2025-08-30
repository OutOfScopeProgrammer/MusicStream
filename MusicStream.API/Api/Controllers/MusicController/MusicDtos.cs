namespace Music.API.Api.Controllers.MusicController;

public record CreateMusicDto(IFormFile File);
public record MusicDto(string Title, string Description, string StreamUrl);


