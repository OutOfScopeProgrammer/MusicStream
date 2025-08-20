using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Music.API.Helper;
using MusicStream.Application.Interfaces.Auth;
using MusicStream.Infrastructure.Auth;

namespace Music.API.Api.Controllers.IdentityController
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IAuthService authService, IOptions<JwtOption> options) : ControllerBase
    {

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] IdentityDto dto)
        {
            var result = await authService.CreateUserWithFreeSubscription(dto.PhoneNumber, dto.Password);
            if (!result.IsSuccess)
                return BadRequest(result.Error);
            CookieHelper.SetCookie(HttpContext, result.Data.AccessToken, options.Value.ExpirationInMinutes);
            return Created();
        }
    }
}
