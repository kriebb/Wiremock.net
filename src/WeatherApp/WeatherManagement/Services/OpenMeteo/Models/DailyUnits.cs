using System.Text.Json.Serialization;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

public record DailyUnits(
    [property: JsonPropertyName("time")] string Time,
    [property: JsonPropertyName("temperature_2m_max")] string Temperature2mMax,
    [property: JsonPropertyName("temperature_2m_min")] string Temperature2mMin
);