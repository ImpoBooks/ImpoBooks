using ErrorOr;
using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.Requests;
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
            User user = registerUserRequest.ToEntity();
            ErrorOr<Success> result = await _usersService.RegisterAsync(user);
            
            return result.Match(
                _ => Results.Created(),
                errors => Results.Conflict(errors.First())
            );
        }
        
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Login([FromBody] LoginUserRequest loginUserRequest)
        {
            ErrorOr<string> result = await _usersService.GenerateJwtAsync(loginUserRequest.Email, loginUserRequest.Password);

            return result.Match(
                _ => Results.Ok(result.Value),
                errors => Results.BadRequest(errors.First())
            );
        }
    }
}