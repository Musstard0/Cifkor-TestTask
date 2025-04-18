// Presenters/WeatherPresenter.cs
using Cysharp.Threading.Tasks;
using System;
using UniRx;

public class WeatherPresenter
{
    private readonly IWeatherService _weatherService;
    private readonly IRequestQueue _requestQueue;
    private readonly IWeatherView _view;
    private readonly CompositeDisposable _disposables = new();
    private IDisposable _timerDisposable;

    public WeatherPresenter(IWeatherService weatherService, IRequestQueue requestQueue, IWeatherView view)
    {
        _weatherService = weatherService;
        _requestQueue = requestQueue;
        _view = view;
    }

    public void OnEnter()
    {
        FetchWeather();
        StartWeatherUpdates();
    }

    public void OnExit()
    {
        StopWeatherUpdates();
    }

    private void StartWeatherUpdates()
    {
        _timerDisposable = Observable.Interval(TimeSpan.FromSeconds(5))
            .Subscribe(_ => FetchWeather())
            .AddTo(_disposables);
    }

    private void StopWeatherUpdates()
    {
        _timerDisposable?.Dispose();
        _disposables.Clear();
        _requestQueue.Cancel(req => req is FetchWeatherRequest);
    }

    private void FetchWeather()
    {
        var request = new FetchWeatherRequest(_weatherService);
        _requestQueue.Enqueue(request).ContinueWith(result =>
        {
            _view.UpdateWeather(result);
        }).Forget();
    }
}
