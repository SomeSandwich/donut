using System.Text.RegularExpressions;

namespace SomeSandwich.Donut.Application.Common.Utils;

/// <summary>
/// Utility class for handling URI operations.
/// </summary>
public static partial class UriUtil
{
    /// <summary>
    /// Regular expression to match and remove 'www.' from the URI.
    /// </summary>
    /// <returns>A Regex object to match 'www.' in URIs.</returns>
    [GeneratedRegex("^https?://(www\\.)?", RegexOptions.IgnoreCase)]
    private static partial Regex RemoveWww();

    /// <summary>
    /// Removes 'www.' from the given URI if present.
    /// </summary>
    /// <param name="uri">The URI string from which 'www.' should be removed.</param>
    /// <returns>The URI string without 'www.'.</returns>
    public static string RemoveWww(this string uri)
    {
        return RemoveWww().Replace(uri, match => match.Groups[1].Success ? match.Value.Replace("www.", "") : match.Value);
    }
}
