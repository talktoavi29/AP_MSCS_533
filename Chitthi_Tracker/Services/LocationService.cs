using Chitthi_Tracker.Models;
using Microsoft.Maui.Devices.Sensors;

namespace Chitthi_Tracker.Services;

public class LocationService : ILocationService
{
    public event EventHandler<LocationPoint>? LocationUpdated;
    public bool IsRunning { get; private set; }

    CancellationTokenSource? _cts;

    public async Task StartAsync(TimeSpan? interval = null, CancellationToken? externalToken = null)
    {
        if (IsRunning) return;
        IsRunning = true;

        _cts = CancellationTokenSource.CreateLinkedTokenSource(externalToken ?? CancellationToken.None);
        var token = _cts.Token;
        var delay = interval ?? TimeSpan.FromSeconds(5);

        // Request permission (Android/iOS)
        var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            IsRunning = false;
            return;
        }

        _ = Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    var location = await Geolocation.GetLocationAsync(request, token);

                    if (location is not null)
                    {
                        var lp = new LocationPoint
                        {
                            Latitude = location.Latitude,
                            Longitude = location.Longitude,
                            AccuracyMeters = location.Accuracy,
                            TimestampUtc = DateTime.UtcNow
                        };
                        LocationUpdated?.Invoke(this, lp);
                    }
                }
                catch { /* swallow transient errors */ }

                try { await Task.Delay(delay, token); } catch { }
            }
        }, token);
    }

    public void Stop()
    {
        if (!IsRunning) return;
        _cts?.Cancel();
        IsRunning = false;
    }
}
