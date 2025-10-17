using Chitthi_Tracker.Services;

namespace Chitthi_Tracker;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiMaps()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ILocationService, LocationService>();
        builder.Services.AddSingleton<IDataStore, SqliteDataStore>();
        builder.Services.AddSingleton<HeatmapService>();

        // Pages
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
    }
}
