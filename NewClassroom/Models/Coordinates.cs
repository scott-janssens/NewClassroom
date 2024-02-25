using System.Text.Json.Serialization;

namespace NewClassroom.Models;

/// <summary>
/// Geographical coordinates.
/// </summary>
/// <param name="Latitude">Latitude value</param>
/// <param name="Longitude">Longitude value</param>
[JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
public record Coordinates(double Latitude, double Longitude);
