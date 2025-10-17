using Chitthi_Tracker.Models;
using SQLite;

namespace Chitthi_Tracker.Services;

public class SqliteDataStore : IDataStore
{
    private SQLiteAsyncConnection? _conn;
    private string DbPath =>
        Path.Combine(FileSystem.AppDataDirectory, "locations.db3");

    public async Task InitializeAsync()
    {
        _conn ??= new SQLiteAsyncConnection(DbPath);
        await _conn.CreateTableAsync<LocationPoint>();
    }

    public async Task InsertAsync(LocationPoint p)
    {
        if (_conn is null) await InitializeAsync();
        await _conn!.InsertAsync(p);
    }

    public async Task<List<LocationPoint>> GetAllAsync()
    {
        if (_conn is null) await InitializeAsync();
        return await _conn!.Table<LocationPoint>().OrderBy(x => x.Id).ToListAsync();
    }

    public async Task<List<LocationPoint>> GetSinceAsync(DateTime sinceUtc)
    {
        if (_conn is null) await InitializeAsync();
        return await _conn!.Table<LocationPoint>()
            .Where(x => x.TimestampUtc >= sinceUtc)
            .OrderBy(x => x.Id)
            .ToListAsync();
    }
}
