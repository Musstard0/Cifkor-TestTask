// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using Newtonsoft.Json.Linq;
using UnityEngine.Networking;
using UnityEngine;

public class WeatherService : IWeatherService
{
    private const string ApiUrl = "https://api.weather.gov/gridpoints/TOP/32,81/forecast";

    public async UniTask<WeatherModel> GetForecastAsync(CancellationToken token)
    {
        using var request = UnityWebRequest.Get(ApiUrl);
        request.SetRequestHeader("User-Agent", "YourAppName/1.0 (your.email@example.com)");
        request.SetRequestHeader("Accept", "application/ld+json");

        await request.SendWebRequest().ToUniTask(cancellationToken: token);

        if (request.result != UnityWebRequest.Result.Success)
            throw new Exception($"Error fetching weather data: {request.error}");

        var json = request.downloadHandler.text;
        var data = JObject.Parse(json);

        Debug.Log(data);
        var periods = data["periods"] as JArray;
        if (periods == null || periods.Count == 0)
            throw new Exception("No forecast data available.");

        var today = periods[0];
        return new WeatherModel
        {
            IconUrl = today["icon"]?.ToString(),
            Temperature = today["temperature"]?.ToString(),
            Description = today["shortForecast"]?.ToString()
        };
    }
}
