using Microsoft.AspNetCore.Mvc;
using Music.API.Helper;
using MusicStream.Application.Interfaces;
using MusicStream.Application.Interfaces.Repositories;

namespace Music.API.Api.Controllers.MusicController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MusicController
    (IWebHostEnvironment env, IMusicChannel musicChannel,
    IMusicRepository musicRepository, LinkGenerator linkGenerator) : ControllerBase
    {

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get list of all musics")]

        public async Task<ActionResult<List<MusicDto>>> GetMusics(CancellationToken cancellationToken)
        {

            var musics = await musicRepository.GetMusics(cancellationToken);
            if (musics!.Count == 0)
                return NotFound(ApiResponse<List<MusicDto>>.NotFound("musics not found."));
            var dtos = MusicDtoMapper.ToMusicDto(musics, linkGenerator, HttpContext);

            var respone = ApiResponse<List<MusicDto>>.Ok(dtos);

            return Ok(respone);
        }
        [HttpGet("{musicId:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Get Music by id")]
        public async Task<ActionResult<MusicDto>> GetMusicById(Guid musicId, CancellationToken cancellationToken)
        {
            var music = await musicRepository.GetMusicById(musicId, true, cancellationToken);
            if (music is null)
                return NotFound(ApiResponse<MusicDto>.NotFound($"music with {musicId} does not exist"));
            var dto = MusicDtoMapper.ToMusicDto(music, linkGenerator, HttpContext);
            return Ok(dto);
        }

        [HttpPost()]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        [EndpointDescription("ایجاد و ارسال موسیقی برای پردازش")]
        [EndpointSummary("Create music")]
        public async Task<IActionResult> CreateMusic([FromForm] CreateMusicDto dto, CancellationToken cancellationToken)
        {

            var (fullPath, fileName, uploadPath) = FileHelper.PrepareFileForSaving(dto.File.FileName, env.WebRootPath);
            Directory.CreateDirectory(uploadPath);

            using var fileStream = System.IO.File.Create(fullPath);
            await dto.File.CopyToAsync(fileStream, cancellationToken);

            var message = new MusicChannelMessage(fullPath, env.WebRootPath, fileName);

            await musicChannel.SendAsync(message);
            return Ok(ApiResponse<IActionResult>.Ok());
        }
    }
}
