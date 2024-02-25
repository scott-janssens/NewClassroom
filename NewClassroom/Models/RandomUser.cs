namespace NewClassroom.Models;

/// <summary>
/// An object containing a payload from the Random User Generator website.
/// </summary>
public record RandomUserResults
{
    /// <summary>
    /// The results collection
    /// </summary>
    public IEnumerable<User>? Results { get; set; }

    /// <summary>
    /// The Info object
    /// </summary>
    public RandomUserInfo? Info { get; set; }
}

/// <summary>
/// The info object in a <see cref="RandomUserResults">RandomUserResults</see> payload.
/// </summary>
public record RandomUserInfo
{
    /// <summary>
    /// The seed used to generate the user results.
    /// </summary>
    public string? Seed { get; set; }

    /// <summary>
    /// The number of results in the payload
    /// </summary>
    public int Results { get; set; }

    /// <summary>
    /// The page number
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// The version
    /// </summary>
    public string? Version { get; set; }
}
