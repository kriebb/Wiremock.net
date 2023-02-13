using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Internal;
using WeatherApp.WeatherManagement.Controllers.Models;
using WeatherApp.WeatherManagement.Services.OpenMeteo;
using WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

namespace WeatherApp.WeatherManagement.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IOpenMeteoClient _meteoClient;
    private readonly ISystemClock _systemClock;

    private static readonly TemperatureCelciusBucket[] TempCBuckets = new[]
    {
        new TemperatureCelciusBucket(new TempCRange(29,100),"Too hot"),
        new TemperatureCelciusBucket(new TempCRange(21,28),"Cozy"),
        new TemperatureCelciusBucket(new TempCRange(15,20),"Cold"),
        new TemperatureCelciusBucket(new TempCRange(-100,15),"Too cold")
    };

    public WeatherForecastController(IOpenMeteoClient meteoClient, ISystemClock systemClock)
    {
        _meteoClient = meteoClient ?? throw new ArgumentNullException(nameof(meteoClient));
        _systemClock = systemClock;
    }

    [HttpGet()]
    public async Task<WeatherForecast> GetAsync()
    {
        var currentForecast = await _meteoClient.GetForecastAsync(DateOnly.FromDateTime(_systemClock.UtcNow.Date));
        if (currentForecast == null) return new WeatherForecast();

        var bucket = TempCBuckets.Single(tempCBucket =>
            currentForecast.CurrentWeather.Temperature >= tempCBucket.minMaxTempC.Min &&
            currentForecast.CurrentWeather.Temperature < tempCBucket.minMaxTempC.Max);

        return new WeatherForecast()
        {
            Date = _systemClock.UtcNow.Date,
            Summary = bucket.Name,
            TemperatureC = currentForecast.CurrentWeather.Temperature
        };
    }
}