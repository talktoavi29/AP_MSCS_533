using Chitthi_Tracker.Models;
using Chitthi_Tracker.Services;
using Microsoft.Maui.Controls;            
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;    
using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;    

namespace Chitthi_Tracker;

public partial class MainPage : ContentPage
{
    readonly ILocationService _loc;
    readonly IDataStore _db;

    private Microsoft.Maui.Controls.Maps.Map _map;

    public MainPage(ILocationService loc, IDataStore db)
    {
        InitializeComponent();
        _loc = loc; _db = db;

        try
        {
            _map = new Microsoft.Maui.Controls.Maps.Map
            {
                MapType = Microsoft.Maui.Maps.MapType.Street,
                IsShowingUser = true
            };
            MapHost.Content = _map;
        }
        catch (Exception ex)
        {
            StatusLbl.Text = $"Map load failed: {ex.Message}";
        }

        _ = _db.InitializeAsync();

        _loc.LocationUpdated += async (_, p) =>
        {
            await _db.InsertAsync(p);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var pin = new Pin
                {
                    Location = new Location(p.Latitude, p.Longitude),
                    Label = $"Fix {DateTime.Now:T}"
                };
                _map.Pins.Add(pin);
                StatusLbl.Text = $"Last fix: {p.Latitude:F5},{p.Longitude:F5} @ {DateTime.Now:T}";
            });
        };
    }

    async void StartClicked(object sender, EventArgs e)
    {
        StartBtn.IsEnabled = false;
        StopBtn.IsEnabled = true;
        StatusLbl.Text = "Tracking…";
        await _loc.StartAsync(TimeSpan.FromSeconds(5));

        var last = (await _db.GetAllAsync()).LastOrDefault();
        if (last != null)
        {
            _map.MoveToRegion(
               Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                    new Location(last.Latitude, last.Longitude),
                    Microsoft.Maui.Maps.Distance.FromKilometers(1)));
        }
    }

    void StopClicked(object sender, EventArgs e)
    {
        _loc.Stop();
        StartBtn.IsEnabled = true;
        StopBtn.IsEnabled = false;
        StatusLbl.Text = "Stopped.";
    }

    async void ShowHistoryClicked(object sender, EventArgs e)
    {
        _map.MapElements.Clear();
        _map.Pins.Clear();

        var points = await _db.GetAllAsync();
        foreach (var p in points)
        {
            _map.Pins.Add(new Pin
            {
                Location = new Location(p.Latitude, p.Longitude),
                Label = p.TimestampUtc.ToLocalTime().ToString("T")
            });
        }

        if (points.Count > 0)
        {
            var last = points.Last();
            _map.MoveToRegion(
                Microsoft.Maui.Maps.MapSpan.FromCenterAndRadius(
                    new Location(last.Latitude, last.Longitude),
                    Microsoft.Maui.Maps.Distance.FromKilometers(2)));
        }
        StatusLbl.Text = $"Rendered {points.Count} points.";
    }
}
