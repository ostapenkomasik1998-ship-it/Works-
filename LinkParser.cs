using System.Diagnostics.CodeAnalysis;

namespace QrScanner.Services;

/// <summary>Чиста (без побічних ефектів) логіка визначення веб-посилань.</summary>
public static class LinkParser
{
    public static bool TryGetWebUri(string? text, [NotNullWhen(true)] out Uri? uri)
    {
        uri = null;
        if (string.IsNullOrWhiteSpace(text)) return false;

        var candidate = text.Trim();
        if (candidate.Any(char.IsWhiteSpace)) return false;

        if (candidate.StartsWith("www.", StringComparison.OrdinalIgnoreCase))
            candidate = "https://" + candidate;

        if (Uri.TryCreate(candidate, UriKind.Absolute, out var parsed)
            && (parsed.Scheme == Uri.UriSchemeHttp || parsed.Scheme == Uri.UriSchemeHttps)
            && !string.IsNullOrEmpty(parsed.Host))
        {
            uri = parsed;
            return true;
        }
        return false;
    }
}
