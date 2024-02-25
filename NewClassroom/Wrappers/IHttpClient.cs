using System.Diagnostics.CodeAnalysis;

namespace NewClassroom.Wrappers;

/// <summary>
/// A wrapper for the HttpClient class.
/// </summary>
public interface IHttpClient
{
    /// <summary>
    /// Sends an HTTP GET request to a Uri.
    /// </summary>
    /// <param name="requestUri">a uri to query.</param>
    /// <returns>An HttpResponseMessage object</returns>
    Task<HttpResponseMessage> GetAsync([StringSyntax(StringSyntaxAttribute.Uri)] string? requestUri);
}
