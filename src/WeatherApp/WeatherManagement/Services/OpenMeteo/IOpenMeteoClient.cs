using WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

namespace WeatherApp.WeatherManagement.Services.OpenMeteo;

public interface IOpenMeteoClient
{
    Task<Forecast?> GetForecastAsync(DateOnly date);
}