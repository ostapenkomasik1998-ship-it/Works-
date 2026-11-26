using QrScanner.Services;
using QrScanner.ViewModels;
using QrScanner.Views;
using ZXing.Net.Maui.Controls;

namespace QrScanner;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeReader(); // ZXing.Net.Maui

        // Services (залежності від абстракцій — Dependency Inversion)
        builder.Services.AddSingleton<IHistoryService>(_ =>
            new JsonHistoryService(Path.Combine(FileSystem.AppDataDirectory, "history.json")));
        builder.Services.AddSingleton<ILinkOpener, BrowserLinkOpener>();
        builder.Services.AddSingleton<IDialogService, ShellDialogService>();

        // ViewModels
        builder.Services.AddTransient<ScanViewModel>();
        builder.Services.AddTransient<HistoryViewModel>();

        // Views
        builder.Services.AddTransient<ScanPage>();
        builder.Services.AddTransient<HistoryPage>();

        return builder.Build();
    }
}
