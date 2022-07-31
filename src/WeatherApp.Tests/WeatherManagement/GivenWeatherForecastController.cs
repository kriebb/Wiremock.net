using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using FakeItEasy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Internal;
using Newtonsoft.Json;
using WeatherApp;
using WeatherApp.WeatherManagement.Controllers.Models;
using WireMock.Admin.Mappings;
using WireMock.Handlers;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Settings;

namespace WeatherApp.Tests.WeatherManagement
{
    public class GivenWeatherForecastController
    {
        public class GivenIntegrationTestsSample
        {


            [Fact]
            [Category("Integration")]
            public async Task WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()
            {
                //Arrange
                var application = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(
                            services => services.AddConfiguredServices());
                    });

                //Act
                var httpClient = application.CreateClient();

                //Assert
                var response = await httpClient.GetAsync("/WeatherForecast");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();

                Assert.NotNull(weather);
                Assert.Equal(DateTime.UtcNow.Date, weather?.Date);
            }
        }

        public class GivenWireMockSample
        {
            [Fact]
            [Category("Integration")]
            public async Task WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()
            {
                //Arrange
                var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start();
                openMeteoWireMockServer.Given(Request.Create()
                    .UsingGet()
                    .WithPath(path => path.Contains("forecast"))
                ).RespondWith(Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBody("{\"latitude\":55.1,\"longitude\":4.96,\"generationtime_ms\":0.38301944732666016,\"utc_offset_seconds\":7200,\"elevation\":3.0,\"current_weather\":{\"temperature\":22.5,\"windspeed\":22.8,\"winddirection\":249.0,\"weathercode\":3.0,\"time\":\"2022-07-31T17:00\"},\"daily_units\":{\"time\":\"iso8601\",\"temperature_2m_max\":\"°C\",\"temperature_2m_min\":\"°C\"},\"daily\":{\"time\":[\"2022-07-31\"],\"temperature_2m_max\":[23.6],\"temperature_2m_min\":[17.0]}}\r\n"));

                var openMeteoHttpClient = openMeteoWireMockServer.CreateClient();

                var fakeHttpClientFactory = new Fake<IHttpClientFactory>();
                fakeHttpClientFactory.CallsTo(httpClientFactory => httpClientFactory.CreateClient("OpenMeteo"))
                    .Returns(openMeteoHttpClient);
                var fakeSystemClock = new Fake<ISystemClock>();
                fakeSystemClock.CallsTo(clock => clock.UtcNow).Returns(new DateTimeOffset(new DateTime(2022, 07, 31)));

                var application = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                services.AddConfiguredServices();
                                services.AddScoped(provider => fakeHttpClientFactory.FakedObject);
                                services.AddScoped(provider => fakeSystemClock.FakedObject);

                            });
                    });

                //Act
                var httpClient = application.CreateClient();

                //Assert
                var response = await httpClient.GetAsync("/WeatherForecast");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();

                Assert.NotNull(weather);
                Assert.Equal(new DateTime(2022, 07, 31), weather?.Date);
            }
        }

        public class GivenWireMockRecordingSample
        {
            [Fact]
            [Category("Integration")]
            public async Task WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()
            {
                //Arrange
                var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start(
                    new WireMockServerSettings()
                    {
                        ProxyAndRecordSettings = new ProxyAndRecordSettings()
                        {
                            Url = "https://api.open-meteo.com",
                            SaveMapping = true,
                            SaveMappingToFile = true,
                            WebProxySettings = new WebProxySettings()
                            {
                                Address = "127.0.0.1:8888"
                            },
                            ExcludedHeaders = new string[]{"Host", "traceparent" }
                        },
                        StartAdminInterface = true,
                        FileSystemHandler = new LocalFileSystemHandler(".")
                    }
                );
  
                var openMeteoHttpClient = openMeteoWireMockServer.CreateClient();

                var fakeHttpClientFactory = new Fake<IHttpClientFactory>();
                fakeHttpClientFactory.CallsTo(httpClientFactory => httpClientFactory.CreateClient("OpenMeteo"))
                    .Returns(openMeteoHttpClient);

                var fakeSystemClock = new Fake<ISystemClock>();
                fakeSystemClock.CallsTo(clock => clock.UtcNow).Returns(new DateTimeOffset(new DateTime(2022, 07, 31)));

                var application = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                services.AddConfiguredServices();
                                services.AddScoped(provider => fakeHttpClientFactory.FakedObject);
                                services.AddScoped(provider => fakeSystemClock.FakedObject);
                            });
                    });

                //Act
                var httpClient = application.CreateClient();

                //Assert
                var response = await httpClient.GetAsync("/WeatherForecast");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();

                Assert.NotNull(weather);
                Assert.Equal(new DateTime(2022, 07, 31), weather?.Date);
            }
        }

        public class GivenWireMockPlaybackSample
        {
            [Fact]
            [Category("Integration")]
            public async Task WhenRequestingCurrentWeatherInformation_DateShouldBeUtcToday()
            {
                //Arrange
                var openMeteoWireMockServer = WireMock.Server.WireMockServer.Start(
                    new WireMockServerSettings()
                    {
                        ReadStaticMappings = true

                    }
                    
                );

                var openMeteoHttpClient = openMeteoWireMockServer.CreateClient();

                var fakeHttpClientFactory = new Fake<IHttpClientFactory>();
                fakeHttpClientFactory.CallsTo(httpClientFactory => httpClientFactory.CreateClient("OpenMeteo"))
                    .Returns(openMeteoHttpClient);

                var fakeSystemClock = new Fake<ISystemClock>();
                fakeSystemClock.CallsTo(clock => clock.UtcNow).Returns(new DateTimeOffset(new DateTime(2022, 07, 31)));

                var application = new WebApplicationFactory<Program>()
                    .WithWebHostBuilder(builder =>
                    {
                        builder.ConfigureServices(
                            services =>
                            {
                                services.AddConfiguredServices();
                                services.AddScoped(provider => fakeHttpClientFactory.FakedObject);
                                services.AddScoped(provider => fakeSystemClock.FakedObject);
                            });
                    });

                //Act
                var httpClient = application.CreateClient();

                //Assert
                var response = await httpClient.GetAsync("/WeatherForecast");
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var weather = await response.Content.ReadFromJsonAsync<WeatherForecast>();

                Assert.NotNull(weather);
                Assert.Equal(new DateTime(2022, 07, 31), weather?.Date);
            }
        }
    }
}