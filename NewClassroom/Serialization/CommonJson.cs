using System.Text.Json;

namespace NewClassroom.Serialization;

/// <summary>
/// Common JSON configurations
/// </summary>
public class CommonJson
{
    /// <summary>
    /// Common options for JSON serialization, including camel casing.
    /// </summary>
    public static readonly JsonSerializerOptions CommonJsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };
}
