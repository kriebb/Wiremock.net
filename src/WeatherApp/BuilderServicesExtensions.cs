using Microsoft.Extensions.Internal;
using WeatherApp.WeatherManagement.Services.OpenMeteo;

namespace WeatherApp;

internal static class BuilderServicesExtensions
{
    public static IServiceCollection AddConfiguredServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddHttpClient("OpenMeteo", httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://api.open-meteo.com");
        });
        services.AddSingleton<ISystemClock, SystemClock>();
        services.AddScoped<IOpenMeteoClient, OpenMeteoClient>();

        return services;
    }
}