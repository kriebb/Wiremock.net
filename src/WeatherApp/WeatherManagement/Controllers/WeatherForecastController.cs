using Microsoft.AspNetCore.Mvc;
using WeatherApp.WeatherManagement.Controllers.Models;
using WeatherApp.WeatherManagement.Services.OpenMeteo;
using WeatherApp.WeatherManagement.Services.OpenMeteo.Models;

namespace WeatherApp.WeatherManagement.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IOpenMeteoClient _meteoClient;

        private static readonly TemperatureCelciusBucket[] TempCBuckets = new[]
        {
            new TemperatureCelciusBucket(new TempCRange(29,100),"Too hot"),
            new TemperatureCelciusBucket(new TempCRange(21,28),"Cozy"),
            new TemperatureCelciusBucket(new TempCRange(15,20),"Cold"),
            new TemperatureCelciusBucket(new TempCRange(-100,15),"Too cold")
        };

        public WeatherForecastController(IOpenMeteoClient meteoClient)
        {
            _meteoClient = meteoClient ?? throw new ArgumentNullException(nameof(meteoClient));
        }

        [HttpGet()]
        public async Task<WeatherForecast> GetAsync()
        {
            var currentForecast = await _meteoClient.GetForecastAsync(DateOnly.FromDateTime(DateTime.Now));
            var bucket = TempCBuckets.Single(tempCBucket =>
                currentForecast.CurrentWeather.Temperature >= tempCBucket.minMaxTempC.Min &&
                currentForecast.CurrentWeather.Temperature < tempCBucket.minMaxTempC.Max);

            return new WeatherForecast()
            {
                Date = DateTime.Now.Date,
                Summary = bucket.Name,
                TemperatureC = currentForecast.CurrentWeather.Temperature

            };
        }
    }
}