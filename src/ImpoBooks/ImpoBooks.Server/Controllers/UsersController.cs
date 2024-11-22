using System.Security.Claims;
using ErrorOr;
using ImpoBooks.Server.Extensions;
using ImpoBooks.Server.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
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