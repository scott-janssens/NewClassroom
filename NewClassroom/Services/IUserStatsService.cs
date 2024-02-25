using NewClassroom.Models;
using static NewClassroom.Services.UserStatsService;

namespace NewClassroom.Services;

/// <summary>
/// Service for processing User data statistics.
/// </summary>
public interface IUserStatsService
{
    /// <summary>
    /// A collection of StatQuery delegates to be processed when creating user statistics
    /// </summary>
    /// <seealso cref="StatQuery"/>
    HashSet<StatQuery> Queries { get; }

    /// <summary>
    /// Adds all default user stat query delegates to the <see cref="Queries">Queries</see> collection.
    /// </summary>
    /// <seealso cref="Queries"/>
    void AddDefaultQueries();

    /// <summary>
    /// Generates a <see cref="StatResults">StatResults</see> object containing statistics generated from the delegates added 
    /// to the <see cref="Queries">Queries</see> collection for the specified users.
    /// </summary>
    /// <param name="users">A collection of User objects</param>
    /// <returns>A StatResults object</returns>
    /// <seealso cref="User"/>
    /// <seealso cref="StatResults"/>
    /// <seealso cref="Queries"/>
    StatResults GetStatistics(IEnumerable<User> users);
}