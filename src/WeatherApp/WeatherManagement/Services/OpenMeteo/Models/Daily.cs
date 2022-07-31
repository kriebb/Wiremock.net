using System.Text.Json.Serialization;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

public record Daily(
    [property: JsonPropertyName("time")] IReadOnlyList<string> Time,
    [property: JsonPropertyName("temperature_2m_max")] IReadOnlyList<double> Temperature2mMax,
    [property: JsonPropertyName("temperature_2m_min")] IReadOnlyList<double> Temperature2mMin
);