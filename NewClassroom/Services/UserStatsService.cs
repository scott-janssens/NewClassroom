using NewClassroom.Enums;
using NewClassroom.Models;

namespace NewClassroom.Services;

/// <summary>
/// Service for processing User data statistics.
/// </summary>
public class UserStatsService : IUserStatsService
{
    #region Constants

    /// <summary>
    /// Identifier for "Percentage of males"
    /// </summary>
    public const string StatMalePct = "Percentage of males";

    /// <summary>
    /// Identifier for "Percentage of females"
    /// </summary>
    public const string StatFemalePct = "Percentage of females";

    /// <summary>
    /// Identifier for "Percentage of first names that start with A-M versus N-Z"
    /// </summary>
    public const string StatFirstNameA_M = "Percentage of first names that start with A-M versus N-Z";

    /// <summary>
    /// Identifier for "Percentage of last names that start with A-M versus N-Z"
    /// </summary>
    public const string StatLastNameA_M = "Percentage of last names that start with A-M versus N-Z";

    /// <summary>
    /// Identifier for "Percentage of people in each state, up to the top 10 most populous states"
    /// </summary>
    public const string StatStatePeople = "Percentage of people in each state, up to the top 10 most populous states";

    /// <summary>
    /// Identifier for "Percentage of females in each state, up to the top 10 most populous states"
    /// </summary>
    public const string StatStateFemale = "Percentage of females in each state, up to the top 10 most populous states";

    /// <summary>
    /// Identifier for "Percentage of males in each state, up to the top 10 most populous states"
    /// </summary>
    public const string StatStateMale = "Percentage of males in each state, up to the top 10 most populous states";

    /// <summary>
    /// Identifier for "Percentage of people in the age range"
    /// </summary>
    public const string StatStateAge = "Percentage of people in the age range";

    #endregion

    /// <summary>
    /// Delegate for a query the returns a <see cref="StatQueryResult">StatQueryResult</see> object.
    /// </summary>
    /// <param name="users">A collcation of User objects</param>
    /// <seealso cref="User"/>
    /// <seealso cref="StatQueryResult"/>
    /// <returns>A StatQueryResult object</returns>
    public delegate StatQueryResult StatQuery(IEnumerable<User> users);

    /// <inheritdoc/>
    public HashSet<StatQuery> Queries { get; } = [];


    /// <inheritdoc/>
    public void AddDefaultQueries()
    {
        Queries.Add(GetGenderQuery(Gender.Male));
        Queries.Add(GetGenderQuery(Gender.Female));
        Queries.Add(GetFirstNameQuery());
        Queries.Add(GetLastNameQuery());
        Queries.Add(GetStatePeopleQuery());
        Queries.Add(GetStateGenderQuery(Gender.Female));
        Queries.Add(GetStateGenderQuery(Gender.Male));
        Queries.Add(GetAgePctQuery());
    }


    /// <inheritdoc/>
    public StatResults GetStatistics(IEnumerable<User> users)
    {
        var stats = new List<StatQueryResult>();

        foreach (var query in Queries)
        {
            var items = query(users);
            stats.Add(items);
        }

        return new(DateTimeOffset.Now, users.Count(), stats);
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the specified gender.
    /// </summary>
    /// <param name="gender">A Gender enum value</param>
    /// <returns>A StatQuery delegate</returns>
    /// <seealso cref="Gender"/>
    /// <seealso cref="StatQuery"/>
    public static StatQuery GetGenderQuery(Gender gender)
    {
        return new StatQuery(
            users =>
            {
                var name = gender == Gender.Male ? StatMalePct : StatFemalePct;
                return new StatQueryResult(name, new List<StatQueryItem>
                {
                    new(name, users.Count(u => u.Gender == gender) / (double)users.Count())
                });
            });
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the user's first name
    /// in the first half of the alphabet.
    /// </summary>
    /// <returns>A StatQuery delegate</returns>
    public static StatQuery GetFirstNameQuery()
    {
        return new StatQuery(
            users => new StatQueryResult(StatFirstNameA_M, new List<StatQueryItem>
            {
                new(StatLastNameA_M,
                    users.Count(u => u.Name.First != null &&
                    CharInRange(char.ToUpper(u.Name.First[0]), 'A', 'M')) / (double)users.Count())
            }));
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the user's last name
    /// in the first half of the alphabet.
    /// </summary>
    /// <returns>A StatQuery delegate</returns>
    public static StatQuery GetLastNameQuery()
    {
        return new StatQuery(
            users => new StatQueryResult(StatLastNameA_M, new List<StatQueryItem>
            {
                new(StatLastNameA_M,
                    users.Count(u => u.Name.Last != null &&
                    CharInRange(char.ToUpper(u.Name.Last[0]), 'A', 'M')) / (double)users.Count())
            }));
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the users in a state.
    /// </summary>
    /// <returns>A StatQuery delegate</returns>
    public static StatQuery GetStatePeopleQuery()
    {
        return new StatQuery(
            users =>
            {
                var stateCounts = users
                    .GroupBy(u => u.Location?.State ?? "Unspecified")
                    .Select<IGrouping<string, User>, (string State, int Count)>(g => (g.Key, g.Count()))
                    .OrderByDescending(x => x.Count)
                    .Take(10);

                var items = stateCounts.Select(s => new StatQueryItem(
                    $"Percentage of people in {s.State}",
                    s.Count / (double)users.Count()));

                return new(StatStatePeople, items);
            });
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the users of a gender
    /// in a state.
    /// </summary>
    /// <returns>A StatQuery delegate</returns>
    public static StatQuery GetStateGenderQuery(Gender gender)
    {
        return new(
            users =>
            {
                var stateCounts = users
                    .GroupBy(u => u.Location?.State ?? "Unspecified")
                    .Select<IGrouping<string, User>, (string State, double GenderPct)>(
                        g => (g.Key, g.Count(x => x.Gender == gender) / (double)g.Count()))
                    .OrderByDescending(x => x.GenderPct)
                    .Take(10);

                var items = stateCounts.Select(s => new StatQueryItem(
                    $"Percentage of {gender.ToString().ToLower()}s in {s.State}",
                    s.GenderPct));

                return new(gender == Gender.Female ? StatStateFemale : StatStateMale, items);
            }
        );
    }

    /// <summary>
    /// Method for getting a delegate that calculates the percentage of the users in an age group.
    /// </summary>
    /// <returns>A StatQuery delegate</returns>
    public static StatQuery GetAgePctQuery()
    {
        return new StatQuery(
            users =>
            {
                var buckets = users
                    .GroupBy(u =>
                    {
                        var key = u.DateOfBirth != null ? (u.DateOfBirth.Age - 1) / 20 : -1;

                        if (key > 5)
                        {
                            key = 5;
                        }

                        return key;
                    })
                    .ToDictionary(g => g.Key, g => g.Count());

                var items = new List<StatQueryItem>();

                if (buckets.TryGetValue(0, out var bucket))
                {
                    items.Add(new("Percentage of people in the age range 0-20",
                        bucket / (double)users.Count()));
                }

                if (buckets.TryGetValue(1, out bucket))
                {
                    items.Add(new("Percentage of people in the age range 21-40",
                        bucket / (double)users.Count()));
                }

                if (buckets.TryGetValue(2, out bucket))
                {
                    items.Add(new("Percentage of people in the age range 41-60",
                        bucket / (double)users.Count()));
                }

                if (buckets.TryGetValue(3, out bucket))
                {
                    items.Add(new("Percentage of people in the age range 61-80",
                        bucket / (double)users.Count()));
                }

                if (buckets.TryGetValue(4, out bucket))
                {
                    items.Add(new("Percentage of people in the age range 81-100",
                        bucket / (double)users.Count()));
                }

                if (buckets.TryGetValue(5, out bucket))
                {
                    items.Add(new("Percentage of people in the age range 100+",
                        bucket / (double)users.Count()));
                }

                return new(StatStateAge, items);
            }
        );
    }

    private static bool CharInRange(char ch, char low, char high) => ch >= low && ch <= high;
}
