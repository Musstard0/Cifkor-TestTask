// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using System.Threading;

public interface IWeatherService
{
    UniTask<WeatherModel> GetForecastAsync(CancellationToken token);
}
