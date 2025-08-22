using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Music.API.Helper;
using MusicStream.Application.Common;
using MusicStream.Application.Services;

namespace Music.API.Api.Controllers.PlaylistController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlaylistController(PlaylistService playlistService, LinkGenerator linkGenerator) : ControllerBase
    {



        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Create a playlist")]

        public async Task<ActionResult<ApiResponse>> CreatePlaylist(CreatePlaylistDto dto, CancellationToken cancellationToken)
        {
            var token = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(token, out var userId);
            var response = await playlistService.CreatePlaylist(userId, cancellationToken, dto.Title);
            if (!response.IsSuccess)
                return BadRequest(ApiResponse.BadRequest(response.Error));

            return Ok(ApiResponse.Ok());
        }

        [HttpGet("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Get Playlist with all of its musics")]
        public async Task<ActionResult<ApiResponse<PlaylistDto>>> GetPlaylistWithMusics(Guid playlistId, CancellationToken cancellationToken)
        {
            var result = await playlistService.GetPlaylistWithMusics(playlistId, cancellationToken);
            if (!result.IsSuccess)
                return ApiResponse<PlaylistDto>.NotFound(ErrorMessages.NotFound(nameof(result.Data)));

            var dto = PlaylistDtoMapper.MapToDto(result.Data, linkGenerator, HttpContext);

            return ApiResponse<PlaylistDto>.Ok(dto);
        }


        [HttpPost("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Add music to playlist")]
        public async Task<ActionResult<ApiResponse>> AddMusicToPlaylist(Guid playlistId, [FromBody] AddMusicToPlaylistDto dto, CancellationToken token)
        {
            var response = await playlistService.AddMusicToPlaylist(dto.musicId, playlistId, token);
            if (!response.IsSuccess)
                return Ok(ApiResponse.NotFound(response.Error));
            return ApiResponse.Ok();
            // TODO: make it in a way that can return badrequest for domain error to
        }


        [HttpDelete("{playlistId:guid}/{musicId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Remove music From playlist")]
        public async Task<ActionResult<ApiResponse>> RemoveMusicToPlaylist(Guid playlistId, Guid musicId, CancellationToken token)
        {
            var response = await playlistService.RemoveMusicFromPlaylist(musicId, playlistId, token);
            if (!response.IsSuccess)
                return Ok(ApiResponse.NotFound(response.Error));
            return ApiResponse.Ok();
        }


        [HttpDelete("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Remove music From playlist")]
        public async Task<ActionResult<ApiResponse<string>>> UpdatePlaylist(Guid playlistId, UpdatePlaylistDto dto, CancellationToken token)
        {
            //  Get user id from token or subscription
            var response = await playlistService.UpdatePlaylist(dto.Title, playlistId, token);
            if (!response.IsSuccess)
                return BadRequest(ApiResponse.BadRequest(response.Error));
            return NoContent();
        }


        [HttpPut("{playlistId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointSummary("Remove playlist")]
        public async Task<ActionResult<ApiResponse>> RemovePlaylist(Guid playlistId, CancellationToken token)
        {
            //  Get user id from token or subscription
            var response = await playlistService.DeletePlaylist(playlistId, token);
            if (!response.IsSuccess)
                return BadRequest(ApiResponse.BadRequest(response.Error));
            return NoContent();
        }
    }
}


