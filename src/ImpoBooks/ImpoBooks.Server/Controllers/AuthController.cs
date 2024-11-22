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

    [HttpPost("signin")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Signin([FromBody] RegisterUserRequest registerUserRequest)
    {
        ErrorOr<string> loginResult;
        User? user = registerUserRequest.ToEntity();
        ErrorOr<Success> registerResult = await _authService.RegisterAsync(user);

        if (registerResult.IsError)
            return Results.BadRequest(registerResult.Errors.First());
            loginResult = await _authService.LoginAsync(registerUserRequest.Email, registerUserRequest.Password);

        string token = loginResult.Value;
        if (token is not null) AppendCookie("necessary-cookies", token);

        return loginResult.Match(
            _ => Results.Created(),
            errors => Results.BadRequest(errors.First())
        );
    }

    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<List<Error>>(StatusCodes.Status400BadRequest)]
    public async Task<IResult> Login([FromBody] LoginUserRequest loginUserRequest)
    {
        ErrorOr<string> result =
            await _authService.LoginAsync(loginUserRequest.Email, loginUserRequest.Password);
        
        string token = result.Value;
        if (token is not null) AppendCookie("necessary-cookies", token);

        return result.Match(
            _ => Results.Ok(),
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