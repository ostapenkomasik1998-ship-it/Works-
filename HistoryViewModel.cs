using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QrScanner.Models;
using QrScanner.Services;

namespace QrScanner.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly IHistoryService _history;
    private readonly ILinkOpener _linkOpener;
    private readonly IDialogService _dialogs;

    public HistoryViewModel(IHistoryService history, ILinkOpener linkOpener, IDialogService dialogs)
    {
        _history = history;
        _linkOpener = linkOpener;
        _dialogs = dialogs;
    }

    public ObservableCollection<ScanRecord> Items { get; } = new();

    [ObservableProperty] private bool _isEmpty = true;

    [RelayCommand]
    private async Task LoadAsync()
    {
        var all = await _history.GetAllAsync();
        Items.Clear();
        foreach (var item in all) Items.Add(item);
        IsEmpty = Items.Count == 0;
    }

    [RelayCommand]
    private async Task OpenAsync(ScanRecord? record)
    {
        if (record is null) return;
        if (!await _linkOpener.OpenAsync(record.Value))
            await _dialogs.AlertAsync("Помилка", "Не вдалося відкрити посилання.");
    }

    [RelayCommand]
    private async Task DeleteAsync(ScanRecord? record)
    {
        if (record is null) return;
        await _history.DeleteAsync(record.Id);
        Items.Remove(record);
        IsEmpty = Items.Count == 0;
    }

    [RelayCommand]
    private async Task ClearAsync()
    {
        if (Items.Count == 0) return;
        if (!await _dialogs.ConfirmAsync("Очистити історію", "Видалити всі записи?", "Так", "Ні")) return;
        await _history.ClearAsync();
        Items.Clear();
        IsEmpty = true;
    }
}
