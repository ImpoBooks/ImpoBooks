using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Requests;

public class RegisterUserRequest
{
    [Required] public string Email { get; set; }
    [Required] public string FullName { get; set; }
    [Required] public string Password { get; set; }
}