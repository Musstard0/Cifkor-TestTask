// Models/WeatherModel.cs
// Core/IWeatherService.cs
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public interface IBreedService
{
    UniTask<List<BreedModel>> GetBreedsAsync(CancellationToken token);
    UniTask<BreedModel> GetBreedDetailAsync(string breedId, CancellationToken token);
}
