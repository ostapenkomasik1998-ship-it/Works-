namespace QrScanner.Services;

public interface IDialogService
{
    Task AlertAsync(string title, string message, string ok = "OK");
    Task<bool> ConfirmAsync(string title, string message, string accept, string cancel);
}
