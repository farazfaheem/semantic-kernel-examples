using FunctionCalling.Models;
using System.Collections.Generic;

namespace FunctionCalling.Services;
internal class WeatherService : IWeatherService
{
    private readonly Dictionary<string, WeatherData> _australianCitiesWeather = new Dictionary<string, WeatherData>(StringComparer.OrdinalIgnoreCase)
    {
        { "Sydney", new WeatherData { City = "Sydney", Temperature = 24, Humidity = 65, Weather = "Partly Cloudy" } },
        { "Melbourne", new WeatherData { City = "Melbourne", Temperature = 20, Humidity = 60, Weather = "Cloudy" } },
        { "Brisbane", new WeatherData { City = "Brisbane", Temperature = 28, Humidity = 70, Weather = "Sunny" } },
        { "Perth", new WeatherData { City = "Perth", Temperature = 31, Humidity = 45, Weather = "Sunny" } },
        { "Adelaide", new WeatherData { City = "Adelaide", Temperature = 26, Humidity = 55, Weather = "Clear" } },
        { "Canberra", new WeatherData { City = "Canberra", Temperature = 22, Humidity = 50, Weather = "Sunny" } },
        { "Hobart", new WeatherData { City = "Hobart", Temperature = 18, Humidity = 65, Weather = "Rainy" } },
        { "Darwin", new WeatherData { City = "Darwin", Temperature = 33, Humidity = 80, Weather = "Stormy" } },
        { "Gold Coast", new WeatherData { City = "Gold Coast", Temperature = 27, Humidity = 75, Weather = "Partly Cloudy" } },
        { "Newcastle", new WeatherData { City = "Newcastle", Temperature = 25, Humidity = 60, Weather = "Sunny" } }
    };

    public Task<WeatherData> GetWeatherDataAsync(string city)
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(GetWeatherDataAsync)} called with city: {city}");

        if (_australianCitiesWeather.TryGetValue(city, out var weatherData))
        {
            return Task.FromResult(weatherData);
        }

        // Return default data for cities not in our dictionary
        return Task.FromResult(new WeatherData
        {
            City = city,
            Temperature = 25,
            Humidity = 60,
            Weather = "Unknown"
        });
    }
}

public interface IWeatherService
{
    Task<WeatherData> GetWeatherDataAsync(string city);
}