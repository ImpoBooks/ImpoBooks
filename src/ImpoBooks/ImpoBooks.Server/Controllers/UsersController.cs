using ErrorOr;
using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.DTOs;
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IResult> Register([FromBody] CreateUserDto createUserDto)
        {
            User user = createUserDto.ToEntity();
            ErrorOr<Success> result = await _usersService.CreateAsync(user);
            
            return result.Match(
                _ => Results.Created(),
                errors => Results.BadRequest(errors.First().Description)
            );
        }
    }
}