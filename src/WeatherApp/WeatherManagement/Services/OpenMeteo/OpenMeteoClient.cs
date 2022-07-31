using WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo;

internal class OpenMeteoClient : IOpenMeteoClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OpenMeteoClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<Forecast?> GetForecastAsync(DateOnly date)
    {
        using var httpClient = _httpClientFactory.CreateClient("OpenMeteo");
        var forecast = await httpClient.GetFromJsonAsync<Forecast>(BuildForecastUri(date));
        return forecast;
    }

    private string BuildForecastUri(DateOnly date)
    {
        return $"/v1/forecast?" +
               "latitude=51.09&" +
               "longitude=4.06&" +
               "daily=temperature_2m_max,temperature_2m_min&" +
               "current_weather=true&" +
               "timezone=Europe%2FBerlin&" +
               $"start_date={date.ToString("yyyy-MM-dd")}&" +
               $"end_date={date.ToString("yyyy-MM-dd")}";
    }
}