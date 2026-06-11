using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Backend.Extensions;

public static class ModelStateExtensions
{
    public static string GetFirstError(this ModelStateDictionary modelState)
    {
        return modelState.Values
            .SelectMany(v => v.Errors)
            .FirstOrDefault()?.ErrorMessage
            ?? "Dữ liệu không hợp lệ";
    }
}