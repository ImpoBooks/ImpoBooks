using ErrorOr;
using ImpoBooks.BusinessLogic.Extensions;
using ImpoBooks.BusinessLogic.Services.Auth;
using ImpoBooks.DataAccess.Entities;
using ImpoBooks.Server.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<List<Error>>(StatusCodes.Status409Conflict)]
    public async Task<IResult> Register([FromBody] RegisterUserRequest registerUserRequest)
    {
        User? user = registerUserRequest.ToEntity();
        ErrorOr<Success> result = await _authService.RegisterAsync(user);

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
            await _authService.GenerateJwtAsync(loginUserRequest.Email, loginUserRequest.Password);
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
}