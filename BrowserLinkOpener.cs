using Microsoft.Maui.ApplicationModel;

namespace QrScanner.Services;

public sealed class BrowserLinkOpener : ILinkOpener
{
    public async Task<bool> OpenAsync(string value)
    {
        if (!LinkParser.TryGetWebUri(value, out var uri)) return false;
        try
        {
            return await Browser.Default.OpenAsync(uri, BrowserLaunchMode.External);
        }
        catch
        {
            return false;
        }
    }
}
