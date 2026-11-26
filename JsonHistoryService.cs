using System.Text.Json;
using QrScanner.Models;

namespace QrScanner.Services;

/// <summary>Зберігає історію сканувань у JSON-файлі у сховищі застосунку.</summary>
public sealed class JsonHistoryService : IHistoryService
{
    private readonly string _filePath;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private List<ScanRecord>? _cache;

    public JsonHistoryService(string filePath) => _filePath = filePath;

    public async Task<IReadOnlyList<ScanRecord>> GetAllAsync()
    {
        await _gate.WaitAsync();
        try
        {
            var list = await LoadAsync();
            return list.OrderByDescending(r => r.ScannedAt).ToList();
        }
        finally { _gate.Release(); }
    }

    public async Task AddAsync(ScanRecord record)
    {
        await _gate.WaitAsync();
        try
        {
            var list = await LoadAsync();
            list.Add(record);
            await SaveAsync(list);
        }
        finally { _gate.Release(); }
    }

    public async Task DeleteAsync(Guid id)
    {
        await _gate.WaitAsync();
        try
        {
            var list = await LoadAsync();
            list.RemoveAll(r => r.Id == id);
            await SaveAsync(list);
        }
        finally { _gate.Release(); }
    }

    public async Task ClearAsync()
    {
        await _gate.WaitAsync();
        try
        {
            var list = await LoadAsync();
            list.Clear();
            await SaveAsync(list);
        }
        finally { _gate.Release(); }
    }

    private async Task<List<ScanRecord>> LoadAsync()
    {
        if (_cache is not null) return _cache;
        if (!File.Exists(_filePath)) return _cache = new List<ScanRecord>();

        try
        {
            await using var stream = File.OpenRead(_filePath);
            _cache = await JsonSerializer.DeserializeAsync<List<ScanRecord>>(stream)
                     ?? new List<ScanRecord>();
        }
        catch (JsonException)
        {
            _cache = new List<ScanRecord>(); // пошкоджений файл — починаємо з порожньої історії
        }
        return _cache;
    }

    private async Task SaveAsync(List<ScanRecord> list)
    {
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, list);
    }
}
