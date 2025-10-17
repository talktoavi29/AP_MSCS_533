using Chitthi_Tracker.Models;

namespace Chitthi_Tracker.Services;

public interface IDataStore
{
    Task InitializeAsync();
    Task InsertAsync(LocationPoint p);
    Task<List<LocationPoint>> GetAllAsync();
    Task<List<LocationPoint>> GetSinceAsync(DateTime sinceUtc);
}
