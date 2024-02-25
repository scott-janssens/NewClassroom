using System.Runtime.Serialization;

namespace NewClassroom.Models;

/// <summary>
/// A root object containing all statistical results.
/// </summary>
[DataContract]
public record StatResults
{
    /// <summary>
    /// The time when the result was generated.
    /// </summary>
    [DataMember]
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// The number of users in the data set.
    /// </summary>
    [DataMember]
    public int UserCount { get; }

    /// <summary>
    /// A collection of <see cref="StatQueryResult">StatQueryResult</see> objects.
    /// </summary>
    [DataMember]
    public IEnumerable<StatQueryResult> Stats { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="timestamp">The time when the result was generated</param>
    /// <param name="userCount">The number of users in the data set</param>
    /// <param name="stats">A collection of StatQueryResult objects</param>
    public StatResults(DateTimeOffset timestamp, int userCount, IEnumerable<StatQueryResult> stats)
    {
        Timestamp = timestamp;
        UserCount = userCount;
        Stats = stats;
    }
}

/// <summary>
/// A user statistic
/// </summary>
[DataContract]
public record StatQueryResult
{
    /// <summary>
    /// The name of the user statistic.
    /// </summary>
    [DataMember]
    public string Name { get; }

    /// <summary>
    /// A collection of statistic items.
    /// </summary>
    [DataMember]
    public IEnumerable<StatQueryItem> Items { get; }

    /// <summary>
    /// Constrctor
    /// </summary>
    /// <param name="name">The name of the user statistic</param>
    /// <param name="items">A collection of statistic items</param>
    public StatQueryResult(string name, IEnumerable<StatQueryItem> items)
    {
        Name = name;
        Items = items;
    }
}

/// <summary>
/// A statistic item, which may be one of a collection
/// </summary>
[DataContract]
public record StatQueryItem
{
    /// <summary>
    /// The description of the item.
    /// </summary>
    [DataMember]
    public string Description { get; }

    /// <summary>
    /// The percentage value.
    /// </summary>
    [DataMember]
    public double Pct { get; }

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="description">The description of the item</param>
    /// <param name="pct">The percentage value</param>
    public StatQueryItem(string description, double pct)
    {
        Description = description;
        Pct = pct;
    }
}