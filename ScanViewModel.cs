using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QrScanner.Models;
using QrScanner.Services;

namespace QrScanner.ViewModels;

public partial class ScanViewModel : ObservableObject
{
    private readonly IHistoryService _history;
    private readonly ILinkOpener _linkOpener;
    private readonly IDialogService _dialogs;

    public ScanViewModel(IHistoryService history, ILinkOpener linkOpener, IDialogService dialogs)
    {
        _history = history;
        _linkOpener = linkOpener;
        _dialogs = dialogs;
    }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasResult))]
    private string? _scannedValue;

    [ObservableProperty] private bool _isDetecting;
    [ObservableProperty] private bool _isLink;

    public bool HasResult => !string.IsNullOrEmpty(ScannedValue);

    /// <summary>Викликається, коли сторінка стає видимою (дозвіл на камеру отримано).</summary>
    public void Resume()
    {
        if (!HasResult) IsDetecting = true;
    }

    /// <summary>Викликається, коли сторінка зникає — камера не аналізує кадри.</summary>
    public void Pause() => IsDetecting = false;

    [RelayCommand]
    private async Task ProcessScanAsync(string? value)
    {
        // Захист від повторних подій, поки результат уже показано
        if (!IsDetecting || string.IsNullOrWhiteSpace(value)) return;

        IsDetecting = false;
        ScannedValue = value;
        IsLink = LinkParser.TryGetWebUri(value, out _);

        await _history.AddAsync(new ScanRecord { Value = value });
    }

    [RelayCommand]
    private async Task OpenLinkAsync()
    {
        if (ScannedValue is null) return;
        if (!await _linkOpener.OpenAsync(ScannedValue))
            await _dialogs.AlertAsync("Помилка", "Не вдалося відкрити посилання.");
    }

    [RelayCommand]
    private void ScanAgain()
    {
        ScannedValue = null;
        IsLink = false;
        IsDetecting = true;
    }
}
