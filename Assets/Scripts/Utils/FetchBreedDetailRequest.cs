using Cysharp.Threading.Tasks;
using System.Threading;

public class FetchBreedDetailRequest : IRequest<BreedModel>
{
    private readonly IBreedService _service;
    private readonly string _breedId;

    public bool IsRunning => throw new System.NotImplementedException();

    public FetchBreedDetailRequest(IBreedService service, string breedId)
    {
        _service = service;
        _breedId = breedId;
    }

    public async UniTask<BreedModel> ExecuteAsync(CancellationToken token)
    {
        return await _service.GetBreedDetailAsync(_breedId, token); // Fetch breed details
    }

    public void Cancel()
    {
        // Implement cancellation logic if needed
    }
}