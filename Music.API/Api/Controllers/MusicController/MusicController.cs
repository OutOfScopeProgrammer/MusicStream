using Microsoft.AspNetCore.Mvc;
using Music.API.Helper;
using MusicStream.Application.Interfaces.Repositories;
using MusicStream.Application.Services;

namespace Music.API.Api.Controllers.MusicController
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class MusicController
    (IWebHostEnvironment env, MusicService musicService,
    IMusicRepository musicRepository, LinkGenerator linkGenerator) : ControllerBase
    {

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointDescription("دریافت لیست تمام موسیقی ها")]
        [EndpointSummary("Get list of all musics")]
        public async Task<ActionResult<List<MusicDto>>> GetMusics(CancellationToken cancellationToken)
        {

            var musics = await musicRepository.GetMusics(cancellationToken);
            if (musics is null)
                return NotFound();
            var dtos = MusicDtoMapper.ToMusicDto(musics, linkGenerator, HttpContext);
            return Ok(dtos);
        }


        [HttpPost()]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [EndpointDescription("ایجاد و ارسال موسیقی برای پردازش")]
        [EndpointSummary("Create music")]
        public async Task<IActionResult> CreateMusic([FromForm] CreateMusicDto dto, CancellationToken cancellationToken)
        {

            var (fullPath, fileName, uploadPath) = FileHelper.PrepareFileForSaving(dto.File.FileName, env.WebRootPath);
            Directory.CreateDirectory(uploadPath);

            using var fileStream = System.IO.File.Create(fullPath);
            await dto.File.CopyToAsync(fileStream);
            var response = await musicService
            .CreateMusic(dto.Title, dto.Description, fullPath, env.WebRootPath,
             fileName, dto.SingerId, cancellationToken);

            return response.IsSuccess
            ? Ok("File is in processing queue")
            : NotFound(response.Error);
        }
    }
}
