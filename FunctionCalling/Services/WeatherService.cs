using FunctionCalling.Models;

namespace FunctionCalling.Services;
internal class WeatherService : IWeatherService
{
    public Task<WeatherData> GetWeatherDataAsync(string city)
    {
        System.Diagnostics.Debug.WriteLine($"{nameof(GetWeatherDataAsync)} called with city: {city}");
        return Task.FromResult(new WeatherData
        {
            City = city,
            Temperature = 30,
            Humidity = 50,
            Weather = "Sunny"
        });
    }
}

public interface IWeatherService
{
    Task<WeatherData> GetWeatherDataAsync(string city);
}