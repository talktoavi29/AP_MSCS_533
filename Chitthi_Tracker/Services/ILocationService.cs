using Chitthi_Tracker.Models;

namespace Chitthi_Tracker.Services;

public interface ILocationService
{
    event EventHandler<LocationPoint>? LocationUpdated;
    bool IsRunning { get; }
    Task StartAsync(TimeSpan? interval = null, CancellationToken? externalToken = null);
    void Stop();
}
