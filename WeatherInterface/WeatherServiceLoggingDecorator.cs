using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace DecoratorDesignPattern.WeatherInterface
{
    public class WeatherServiceLoggingDecorator : IWeatherService
    {
        private IWeatherService _innerWeatherService;
        private ILogger<WeatherServiceLoggingDecorator> _logger;

        public WeatherServiceLoggingDecorator(IWeatherService innerWeatherService, ILogger<WeatherServiceLoggingDecorator> logger)
        {
            _innerWeatherService = innerWeatherService;
            _logger = logger;
        }

        public CurrentWeather GetCurrentWeather(string location)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            CurrentWeather currentWeather = _innerWeatherService.GetCurrentWeather(location);
            stopwatch.Stop();
            long elapsedMillis = stopwatch.ElapsedMilliseconds;
            _logger.LogWarning("Retrieved weather data for {location} - Elapsed ms: {} {@currentWeather}", location, elapsedMillis, currentWeather);

            return currentWeather;
        }

        public LocationForecast GetForecast(string location)
        {
            Stopwatch sw = Stopwatch.StartNew();
            LocationForecast forecast = _innerWeatherService.GetForecast(location);
            sw.Stop();
            long elapsedMillis = sw.ElapsedMilliseconds;
            _logger.LogWarning("Retrieved forecast data for {location} - Elapsed ms: {} {@currentWeather}", location, elapsedMillis, forecast);

            return _innerWeatherService.GetForecast(location);
        }
    }
}

