using System.Text.Json.Serialization;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

public record CurrentWeather(
    [property: JsonPropertyName("temperature")] double Temperature,
    [property: JsonPropertyName("windspeed")] double Windspeed,
    [property: JsonPropertyName("winddirection")] double Winddirection,
    [property: JsonPropertyName("weathercode")] double Weathercode,
    [property: JsonPropertyName("time")] string Time
);