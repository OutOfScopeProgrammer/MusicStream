using Music.API.Api.Controllers.MusicController;

namespace Music.API.Api.Controllers.PlaylistController;

public record PlaylistDto(string Title, List<MusicDto> MusicDtos);
public record AddMusicToPlaylistDto(Guid musicId);
