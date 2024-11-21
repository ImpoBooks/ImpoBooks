using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;

namespace ImpoBooks.BusinessLogic.Services.Extensions;

internal static class HttpResponseMessageExtension
{
    public static async Task<ProblemDetails> GetProblemDetailsAsync(this HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return null;

        ProblemDetails errorOr = await response.Content.ReadFromJsonAsync<ProblemDetails>();
        return errorOr;
    }
}

