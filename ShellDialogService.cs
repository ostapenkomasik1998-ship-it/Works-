namespace QrScanner.Services;

public sealed class ShellDialogService : IDialogService
{
    public Task AlertAsync(string title, string message, string ok = "OK")
        => Shell.Current.DisplayAlert(title, message, ok);

    public Task<bool> ConfirmAsync(string title, string message, string accept, string cancel)
        => Shell.Current.DisplayAlert(title, message, accept, cancel);
}
