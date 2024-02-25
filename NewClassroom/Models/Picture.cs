namespace NewClassroom.Models;

/// <summary>
/// Contains urls for user images.
/// </summary>
/// <param name="Large">Large image url</param>
/// <param name="Medium">Medium image url</param>
/// <param name="Thumbnail">Thumbnail image url</param>
public record Picture(
    string? Large,
    string? Medium,
    string? Thumbnail
);
