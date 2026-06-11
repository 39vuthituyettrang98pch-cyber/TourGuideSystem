using System.Text.Json;

namespace Backend.Extensions;

public static class DebugExtensions
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
    };

    /// <summary>
    /// In object ra Console dưới dạng JSON đẹp.
    /// </summary>
    public static void Dump(this object? obj)
    {
        Console.WriteLine("==========Debug==========");
        if (obj == null)
        {
            Console.WriteLine("[NULL]");
            return;
        }
        Console.WriteLine(JsonSerializer.Serialize(obj, JsonOptions));
        Console.WriteLine("==========Debug==========");
    }
}