namespace NewClassroom.Models;

/// <summary>
/// The user's location info.
/// </summary>
/// <param name="Address">A Street object</param>
/// <param name="City">City name</param>
/// <param name="State">State name</param>
/// <param name="Country">Country name</param>
/// <param name="PostCode">Post Code</param>
/// <param name="Coordinates">A Coordinate object</param>
/// <param name="Timezone">A Timezone object</param>
/// <seealso cref="Street"/>
/// <seealso cref="Coordinates"/>
/// <seealso cref="Timezone"/>
public record Location(
    Street? Address, 
    string? City, 
    string? State,
    string? Country, 
    int? PostCode,
    Coordinates? Coordinates,
    Timezone? Timezone);
