using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using NewClassroom.Models;
using System.Text;

namespace NewClassroom.Serialization.Formatters;

/// <summary>
/// Formatter to output a <see cref="StatResults">StatResults</see> object in plain text.
/// </summary>
public class PlainTextOutputFormatter : TextOutputFormatter
{
    /// <summary>
    /// Constructor
    /// </summary>
    public PlainTextOutputFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/plain"));

        SupportedEncodings.Add(Encoding.UTF8);
        SupportedEncodings.Add(Encoding.Unicode);
    }

    /// <inheritdoc/>
    protected override bool CanWriteType(Type? type) => typeof(StatResults).IsAssignableFrom(type);

    /// <inheritdoc/>
    public override async Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
    {
        var resultsString = new StringBuilder();
        var httpContext = context.HttpContext;
        var results = context.Object as StatResults;

        if (results != null)
        {
            resultsString.AppendLine($"Timestamp: {results.Timestamp}");
            resultsString.AppendLine($"User Count: {results.UserCount}\n");

            foreach (var stat in results.Stats)
            {
                if (stat.Items.Count() == 1)
                {
                    WriteItem(resultsString, stat.Items.First());
                }
                else
                {
                    resultsString.AppendLine(stat.Name);

                    foreach (var item in stat.Items)
                    {
                        WriteItem(resultsString, item, 1);
                    }
                }
            }
        }

        await httpContext.Response.WriteAsync(resultsString.ToString(), selectedEncoding);

        static void WriteItem(StringBuilder resultsString, StatQueryItem item, int tab = 0)
        {
            for (int i = 0; i < tab; i++)
            {
                resultsString.Append('\t');
            }
            resultsString.Append(item.Description);
            resultsString.Append(": ");
            resultsString.Append((item.Pct * 100).ToString("F2"));
            resultsString.AppendLine("%");
        }
    }
}
