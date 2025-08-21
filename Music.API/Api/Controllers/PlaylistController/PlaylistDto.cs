using Music.API.Api.Controllers.MusicController;

namespace Music.API.Api.Controllers.PlaylistController;

public record class PlaylistDto(string Title, List<MusicDto> MusicDtos);
