using Microsoft.Build.Framework;

namespace ImpoBooks.Server.Requests;

public class LoginUserRequest
{
    [Required] public string Email { get; set; }
    [Required] public string Password { get; set; }
}