using QrScanner.Models;

namespace QrScanner.Services;

public interface IHistoryService
{
    Task<IReadOnlyList<ScanRecord>> GetAllAsync();
    Task AddAsync(ScanRecord record);
    Task DeleteAsync(Guid id);
    Task ClearAsync();
}
