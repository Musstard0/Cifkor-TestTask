// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class FetchBreedsRequest : IRequest<List<BreedModel>>
{
    private readonly IBreedService _service;
    public bool IsRunning { get; private set; }

    public FetchBreedsRequest(IBreedService service)
    {
        _service = service;
    }

    public async UniTask<List<BreedModel>> ExecuteAsync(CancellationToken token)
    {
        IsRunning = true;
        try
        {
            return await _service.GetBreedsAsync(token);
        }
        finally
        {
            IsRunning = false;
        }
    }

    public void Cancel() { /* Implement cancel logic if needed */ }
}

