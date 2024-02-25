using Microsoft.AspNetCore.Mvc;
using NewClassroom.Models;
using NewClassroom.Serialization;
using NewClassroom.Services;
using NewClassroom.Wrappers;
using System.Text.Json;

namespace NewClassroom.Controllers
{
    /// <summary>
    /// Controller for User statistics endpoints.
    /// </summary>
    /// <param name="service">a UserStatsService object</param>
    /// <param name="httpClient">An HttpClient object</param>
    /// <param name="logger">A logger object</param>
    [ApiController]
    [Route("api/[controller]")]
    public class UserStatsController(IUserStatsService service, IHttpClient httpClient, ILogger<UserStatsController> logger) : ControllerBase
    {
        /// <summary>
        /// Calculates user statistics from provided data from Random User Generator.
        /// </summary>
        /// <param name="randomUserData">A RandomUserResults object</param>
        /// <returns>Ok action if successful</returns>
        [HttpPut]
        public IActionResult Put(RandomUserResults randomUserData)
        {
            if (randomUserData.Results == null || !randomUserData.Results.Any())
            {
                return BadRequest();
            }

            service.AddDefaultQueries();
            var results = service.GetStatistics(randomUserData.Results);
            return Ok(results);
        }

        /// <summary>
        /// Fetches a specified number of users from Random User Generator and returns statistics for them.
        /// </summary>
        /// <param name="users">The number of users to retrieve</param>
        /// <returns>Ok action if successful</returns>
        [HttpGet] 
        public async Task<IActionResult> Get(int users) 
        {
            var response = await httpClient.GetAsync($"https://randomuser.me/api/?nat=us&results={users}");

            if (response?.IsSuccessStatusCode ?? false)
            {
                var contentString = await response.Content.ReadAsStringAsync();
                var randomUsers = JsonSerializer.Deserialize<RandomUserResults>(contentString, CommonJson.CommonJsonOptions);

                if (randomUsers?.Results != null)
                {
                    service.AddDefaultQueries();
                    var results = service.GetStatistics(randomUsers.Results);
                    return Ok(results);
                }
            }

            return Problem(); 
        }
    }
}
