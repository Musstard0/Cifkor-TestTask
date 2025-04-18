// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using System.Threading;
// Core/FetchWeatherRequest.cs
public class FetchWeatherRequest : IRequest<WeatherModel>
{
    private readonly IWeatherService _service;
    private CancellationTokenSource _cts;

    public bool IsRunning { get; private set; }

    public FetchWeatherRequest(IWeatherService service)
    {
        _service = service;
    }

    public async UniTask<WeatherModel> ExecuteAsync(CancellationToken token)
    {
        IsRunning = true;
        _cts = CancellationTokenSource.CreateLinkedTokenSource(token);

        try
        {
            return await _service.GetForecastAsync(_cts.Token);
        }
        finally
        {
            IsRunning = false;
        }
    }

    public void Cancel()
    {
        _cts?.Cancel();
    }
}
