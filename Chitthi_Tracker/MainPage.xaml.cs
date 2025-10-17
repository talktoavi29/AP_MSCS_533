using Chitthi_Tracker.Models;
using Chitthi_Tracker.Services;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;

namespace Chitthi_Tracker;

public partial class MainPage : ContentPage
{
    readonly ILocationService _loc;
    readonly IDataStore _db;
    readonly HeatmapService _heat;

    public MainPage(ILocationService loc, IDataStore db, HeatmapService heat)
    {
        InitializeComponent();     // <-- will appear once XAML .g.cs is generated
        _loc = loc; _db = db; _heat = heat;

        _ = _db.InitializeAsync();

        _loc.LocationUpdated += async (_, p) =>
        {
            await _db.InsertAsync(p);
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var circle = new MapCircle
                {
                    Center = new Location(p.Latitude, p.Longitude),
                    Radius = Distance.FromMeters(40),
                    StrokeColor = Colors.Transparent,
                    FillColor = Color.FromRgba(255, 0, 0, 0.18)
                };
                MyMap.MapElements.Add(circle);
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
        // Center once on first start
        var last = (await _db.GetAllAsync()).LastOrDefault();
        if (last != null)
            MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(new Location(last.Latitude, last.Longitude),
				Distance.FromKilometers(2)));

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
        // Clear and redraw all saved points as heat circles
        MyMap.MapElements.Clear();
        var points = await _db.GetAllAsync();
        foreach (var el in _heat.BuildCircles(points))
            MyMap.MapElements.Add(el);

        if (points.Count > 0)
        {
            var last = points.Last();
            MyMap.MoveToRegion(
						MapSpan.FromCenterAndRadius(new Location(last.Latitude, last.Longitude),
						Distance.FromKilometers(2)));

        }
        StatusLbl.Text = $"Rendered {points.Count} points.";
    }
}
