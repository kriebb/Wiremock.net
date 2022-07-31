using System.Text.Json.Serialization;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

public record Forecast(
    [property: JsonPropertyName("latitude")] double Latitude,
    [property: JsonPropertyName("longitude")] double Longitude,
    [property: JsonPropertyName("generationtime_ms")] double GenerationtimeMs,
    [property: JsonPropertyName("utc_offset_seconds")] int UtcOffsetSeconds,
    [property: JsonPropertyName("elevation")] double Elevation,
    [property: JsonPropertyName("current_weather")] CurrentWeather CurrentWeather,
    [property: JsonPropertyName("daily_units")] DailyUnits DailyUnits,
    [property: JsonPropertyName("daily")] Daily Daily
);