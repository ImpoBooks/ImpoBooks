using System.Security.Claims;
using ErrorOr;
using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Requests;
using ImpoBooks.Server.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IAuthService _authService;

        public UsersController(IUsersService usersService, IAuthService authService)
        {
            _usersService = usersService;
            _authService = authService;
        }

        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType<UserProfileResponse>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
        public IResult GetProfile()
        {
            UserProfileResponse userProfileResponse = new()
            {
                Id = User.FindFirst("id")?.Value,
                Name = User.FindFirst("name")?.Value,
                Email = User.FindFirst(ClaimTypes.Email)?.Value,
            };

            ErrorOr<Success> result = userProfileResponse.CheckPropertiesForNull();

            return result.Match(
                _ => Results.Ok(userProfileResponse),
                errors => Results.BadRequest(errors.First())
            );
        }

        [HttpPatch("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
        public async Task<IResult> UpdateProfile([FromBody] UpdateUserRequest request)
        {
            int userId = Int32.Parse(User.FindFirst("id")?.Value);
            if (userId <= 0 || userId == null) return Results.Unauthorized();

            ErrorOr<User> result = await _usersService.UpdateUserAsync(userId, request.Name, request.Password);
            if (!result.IsError)
            {
                ErrorOr<string> token = _authService.GenerateJwt(result.Value);
                if (!token.IsError)
                {
                    HttpContext.Response.Cookies.Delete("necessary-cookies");
                    AppendCookie("necessary-cookies", token.Value);
                }
            }

            return result.Match(
                _ => Results.NoContent(),
                errors => Results.BadRequest(errors.First())
            );
        }

        [HttpDelete("me")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
        public async Task<IResult> DeleteProfile()
        {
            var userId = Int32.Parse(User.FindFirst("id")?.Value);
            if (userId == null || userId <= 0) return Results.Unauthorized();

            var result = await _usersService.DeleteUserAsync(userId);
            HttpContext.Response.Cookies.Delete("necessary-cookies");

            return result.Match(
                _ => Results.NoContent(),
                errors => Results.BadRequest(errors.First())
            );
        }

        private void AppendCookie(string key, string token)
        {
            HttpContext.Response.Cookies.Append(key, token,
                new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None
                });
        }
    }
}