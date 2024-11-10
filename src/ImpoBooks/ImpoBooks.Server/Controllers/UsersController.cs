using System.Security.Claims;
using ErrorOr;
using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services;
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
    public class UsersController(IUsersService usersService) : ControllerBase
    {
        private readonly IUsersService _usersService = usersService;

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status409Conflict)]
        public async Task<IResult> Register([FromBody] RegisterUserRequest registerUserRequest)
        {
            User? user = registerUserRequest.ToEntity();
            ErrorOr<Success> result = await _usersService.RegisterAsync(user);

            return result.Match(
                _ => Results.Created(),
                errors => Results.Conflict(errors.First())
            );
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            ErrorOr<string> result =
                await _usersService.GenerateJwtAsync(loginUserRequest.Email, loginUserRequest.Password);
            string token = result.Value;

            if (token is not null)
                HttpContext.Response.Cookies.Append("necessary-cookies", token,
                    new CookieOptions()
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.None
                    });

            return result.Match(
                _ => Results.Ok(),
                errors => Results.BadRequest(errors.First())
            );
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
    }
}