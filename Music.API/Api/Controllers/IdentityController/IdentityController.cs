using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Music.API.Helper;
using MusicStream.Application.Interfaces.Auth;
using MusicStream.Infrastructure.Auth;

namespace Music.API.Api.Controllers.IdentityController
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController(IAuthService authService, LinkGenerator linkGenerator) : ControllerBase
    {
        // TODO:Send refreshToken inside cookie and jwt in the response

        [HttpPost("sign-up")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Sign Up")]
        public async Task<ActionResult<ApiResponse<IdentityResponse>>> SignUp([FromBody] IdentityDto dto, CancellationToken cancellationToken)
        {
            var result = await authService.CreateUserWithFreeSubscription(dto.PhoneNumber, dto.Password, cancellationToken);
            if (!result.IsSuccess)
                return BadRequest(ApiResponse<IActionResult>.BadRequest(result.Error));

            CookieHelper.SetCookie(HttpContext, result.Data.RefreshToken.Token, result.Data.RefreshToken.ExpirationTime, linkGenerator);

            return Ok(ApiResponse<IdentityResponse>.Ok(new IdentityResponse(result.Data.AccessToken)));
        }
        [HttpPost("sign-in")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Sign In")]
        public async Task<ActionResult<ApiResponse<IdentityResponse>>> SignIn(IdentityDto dto, CancellationToken cancellationToken)
        {
            var result = await authService.LoginWithUserNameAndPassword(dto.PhoneNumber, dto.Password, cancellationToken);
            if (!result.IsSuccess)
                return Unauthorized();

            CookieHelper.SetCookie(HttpContext, result.Data.RefreshToken.Token, result.Data.RefreshToken.ExpirationTime, linkGenerator);
            var response = new IdentityResponse(result.Data.AccessToken);
            return ApiResponse<IdentityResponse>.Ok(response);

        }
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [EndpointSummary("Refresh Token")]
        // WARNING: Dont remove EndpointName. needed for set the cookie
        [EndpointName("refresh-token-endpoint")]
        public async Task<ActionResult<ApiResponse<IdentityResponse>>> SignInRefreshToken(CancellationToken cancellationToken)
        {
            var cookie = CookieHelper.GetRefreshToken(HttpContext);
            var result = await authService.LoginUsingRefreshToken(cookie, cancellationToken);
            if (!result.IsSuccess)
                return Unauthorized();
            CookieHelper.SetCookie(HttpContext, result.Data.RefreshToken.Token, result.Data.RefreshToken.ExpirationTime, linkGenerator);
            var response = new IdentityResponse(result.Data.AccessToken);
            return ApiResponse<IdentityResponse>.Ok(response);
        }
    }
}
