using System.Reflection;
using ErrorOr;
using ImpoBooks.BusinessLogic.Errors.Users;
using ImpoBooks.Server.Responses;

namespace ImpoBooks.BusinessLogic.Extensions;

public static class UserProfileExtensions
{
    
    public static ErrorOr<Success> CheckPropertiesForNull(this UserProfileResponse userProfileResponse)
    {
        List<string> emptyProperties = new();
        foreach(PropertyInfo propertyInfo in userProfileResponse.GetType().GetProperties())
        {
            if(propertyInfo.PropertyType == typeof(string))
            {
                string value = (string)propertyInfo.GetValue(userProfileResponse);
                if(string.IsNullOrEmpty(value)) emptyProperties.Add(propertyInfo.Name);
            }
        } 
        
        return emptyProperties.Any() ? UserErrors.NullProperties(emptyProperties) : Result.Success;
    }
}