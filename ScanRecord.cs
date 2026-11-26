using System.Text.Json.Serialization;
using QrScanner.Services;

namespace QrScanner.Models;

/// <summary>Один запис в історії сканувань.</summary>
public class ScanRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Value { get; set; } = string.Empty;
    public DateTimeOffset ScannedAt { get; set; } = DateTimeOffset.Now;

    [JsonIgnore]
    public bool IsLink => LinkParser.TryGetWebUri(Value, out _);
}
