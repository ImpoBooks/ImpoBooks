using System.ComponentModel.DataAnnotations;

namespace ImpoBooks.Server.Requests;

public class UpdateUserRequest
{
    public string? Name { get; set; }
    public string? Password { get; set; }
}
