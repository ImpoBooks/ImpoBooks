using ErrorOr;

namespace ImpoBooks.BusinessLogic.Errors.Users;

public static class UserErrors
{
    public static Error AlreadyExists =>
        Error.Custom(ErrorTypes.AlreadyExists, "User.Exists", "User with this email already exists.");
    
    public static Error IsNull =>
        Error.Custom(ErrorTypes.IsNull, "User.IsNull", "User was null.");
}