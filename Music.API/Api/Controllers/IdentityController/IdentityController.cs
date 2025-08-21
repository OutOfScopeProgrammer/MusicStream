using Microsoft.AspNetCore.Http.HttpResults;
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Sign Up")]
        public async Task<ActionResult<IdentityResponse>> SignUp([FromBody] IdentityDto dto, CancellationToken cancellationToken)
        {
            var result = await authService.CreateUserWithFreeSubscription(dto.PhoneNumber, dto.Password, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<IActionResult>.BadRequest(result.Error));
            CookieHelper.SetCookie(HttpContext, result.Data.AccessToken, options.Value.ExpirationInMinutes);
            // TODO: return refreshToken
            return Ok(ApiResponse<IdentityResponse>.Ok(new IdentityResponse(result.Data.RefreshToken)));
        }
    }
}
