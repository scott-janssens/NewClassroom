using NewClassroom.Enums;
using System.Text.Json.Serialization;

namespace NewClassroom.Models;

/// <summary>
/// Represents a User and their assorted information.
/// </summary>
/// <param name="Name">The user's name [required]</param>
/// <param name="Id">An Identification object with the user's ID [required]</param>
/// <seealso cref="Gender"/>
/// <seealso cref="Location"/>
/// <seealso cref="Login"/>
/// <seealso cref="AgeDate"/>
/// <seealso cref="Picture"/>
public record User(Name Name, Identification Id)
{
    /// <summary>
    /// The user's gender.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Gender? Gender { get; set; }
    
    /// <inheritdoc/>
    public Location? Location { get; set; }
    
    /// <summary>
    /// The user's email address.
    /// </summary>
    public string? Email { get; set; }

    /// <inheritdoc/>
    public Login? Login { get; set; }

    /// <summary>
    /// The user's birth date.
    /// </summary>
    [JsonPropertyName("dob")]
    public AgeDate? DateOfBirth { get; set; }
    
    /// <summary>
    /// The user's registration date.
    /// </summary>
    public AgeDate? Registered { get; set; }

    /// <summary>
    /// The user's phone number.
    /// </summary>
    public string? Phone { get; set; }
    
    /// <summary>
    /// The user's cell phone number.
    /// </summary>
    public string? Cell { get; set; }

    /// <inheritdoc/>
    public Picture? Picture { get; set; }
    
    /// <summary>
    /// The user's nationality.
    /// </summary>
    [JsonPropertyName("nat")]
    public string? Nationality { get; set; }
}
