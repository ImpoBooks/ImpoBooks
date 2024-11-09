using System.Text;
using ErrorOr;

namespace ImpoBooks.BusinessLogic.Errors.Users;

public static class UserErrors
{
    public static Error AlreadyExists =>
        Error.Custom(ErrorTypes.AlreadyExists, "User.Exists", "User with this email already exists.");

    public static Error IsNull =>
        Error.Custom(ErrorTypes.IsNull, "User.IsNull", "User was null.");

    public static Error NotFoundByEmail =>
        Error.Custom(ErrorTypes.NotFound, "User.NotFoundByEmail", "User with this email does not exist.");

    public static Error WrongPassword =>
        Error.Custom(ErrorTypes.WrongInfo, "User.WrongPassword", "Password was invalid.");

    public static Error NullProperties(List<string> propertiesNames)
    {
        StringBuilder errorDesription = new StringBuilder();
        errorDesription.Append("Properties can not be null or empty:");

        foreach (var propertyName in propertiesNames)
            errorDesription.Append($" {propertyName},");

        return Error.Custom(ErrorTypes.NullProperties, "User.NullProperties", errorDesription.ToString().TrimEnd(','));
    }
}