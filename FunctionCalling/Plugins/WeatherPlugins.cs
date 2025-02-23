using System.ComponentModel;
using System.Diagnostics;
using FunctionCalling.Models;
using FunctionCalling.Services;
using Microsoft.SemanticKernel;

namespace FunctionCalling.Plugins;
public class WeatherPlugins(IWeatherService weatherService)
{
    //its a Semantic Kernel Plugin class for weather 

    [KernelFunction("get_weather_data_by_city")]
    [Description("Get the weather data by city")]
    public async Task<WeatherData> GetWeatherData(string city)
    {
        return await weatherService.GetWeatherDataAsync(city);
    }
}
