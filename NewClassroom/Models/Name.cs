namespace NewClassroom.Models;

/// <summary>
/// Represents a user's name.
/// </summary>
/// <param name="Title">Title</param>
/// <param name="First">First name</param>
/// <param name="Last">Last name</param>
public record Name(string? Title, string? First, string? Last);
