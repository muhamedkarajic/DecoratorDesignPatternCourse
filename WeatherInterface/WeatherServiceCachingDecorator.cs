using System;
using System.Diagnostics;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace DecoratorDesignPattern.WeatherInterface
{
    public class WeatherServiceCachingDecorator : IWeatherService
    {
        private IWeatherService _innerWeatherService;
        private IMemoryCache _cache;

        public WeatherServiceCachingDecorator(IWeatherService innerWeatherService, IMemoryCache cache)
        {
            _innerWeatherService = innerWeatherService;
            _cache = cache;
        }

        public CurrentWeather GetCurrentWeather(string location)
        {
            string cacheKey = $"WeatherConditions::{location}";
            if (_cache.TryGetValue<CurrentWeather>(cacheKey, out var currentWeather)) return currentWeather;
            var currentConditions = _innerWeatherService.GetCurrentWeather(location);
            _cache.Set<CurrentWeather>(cacheKey, currentConditions, TimeSpan.FromMinutes(30));
            return currentConditions;
        }

        public LocationForecast GetForecast(string location)
        {
            string cacheKey = $"WeatherForecast::{location}";
            if (_cache.TryGetValue<LocationForecast>(cacheKey, out var forecast)) return forecast;
            var locationForecast = _innerWeatherService.GetForecast(location);
            _cache.Set<LocationForecast>(cacheKey, locationForecast, TimeSpan.FromMinutes(30));
            return locationForecast;
        }
    }
}

