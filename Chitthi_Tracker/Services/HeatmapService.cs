// using Chitthi_Tracker.Models;
// using Microsoft.Maui.Controls.Maps;
// using Microsoft.Maui.Maps;

// namespace Chitthi_Tracker.Services;

// public class HeatmapService
// {
//     // You can tweak these to change look/feel
//     const double BaseRadiusMeters = 40;      // circle radius per point
//     const double BaseAlpha = 0.18;           // transparency per circle

//     public IEnumerable<MapElement> BuildCircles(IEnumerable<LocationPoint> points)
//     {
//         foreach (var p in points)
//         {
//             yield return new MapCircle
//             {
//                 Center = new Location(p.Latitude, p.Longitude),
//                 Radius = Distance.FromMeters(BaseRadiusMeters),
//                 StrokeColor = Colors.Transparent,
//                 FillColor = Color.FromRgba(255, 0, 0, BaseAlpha) // reddish heat
//             };
//         }
//     }
// }
