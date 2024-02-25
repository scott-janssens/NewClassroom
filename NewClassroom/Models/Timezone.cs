namespace NewClassroom.Models;

/// <summary>
/// Represents a timezone.
/// </summary>
/// <param name="Offset">The timezone offset</param>
/// <param name="Description">A description of the timezone</param>
public record Timezone(string Offset, string? Description);
