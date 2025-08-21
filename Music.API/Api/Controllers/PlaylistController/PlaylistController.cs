using Microsoft.AspNetCore.Mvc;
using Music.API.Helper;
using MusicStream.Application.Common;
using MusicStream.Application.Services;

namespace Music.API.Api.Controllers.PlaylistController
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaylistController(PlaylistService playlistService, LinkGenerator linkGenerator) : ControllerBase
    {

        [HttpGet("{playlistId:guid}")]
        public async Task<ActionResult<ApiResponse<PlaylistDto>>> GetPlaylistWithMusics(Guid playlistId, CancellationToken cancellationToken)
        {
            var result = await playlistService.GetPlaylistWithMusics(playlistId, cancellationToken);
            if (!result.IsSuccess)
                return ApiResponse<PlaylistDto>.NotFound(ErrorMessages.NotFound(nameof(result.Data)));

            var dto = PlaylistDtoMapper.MapToDto(result.Data, linkGenerator, HttpContext);

            return ApiResponse<PlaylistDto>.Ok(dto);
        }
    }
}
