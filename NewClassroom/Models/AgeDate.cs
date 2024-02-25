namespace NewClassroom.Models;

/// <summary>
/// Object that stores a date and the number of years since.
/// </summary>
/// <param name="Date">A date value</param>
/// <param name="Age">Number of years from Date until now.</param>
public record AgeDate(DateTimeOffset Date, int Age);
