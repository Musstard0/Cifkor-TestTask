using Cysharp.Threading.Tasks;
using UniRx;

public class BreedPresenter
{
    readonly IBreedService _service;
    readonly IRequestQueue _queue;
    readonly IBreedView _view;

    // Constructor: injected via Zenject
    public BreedPresenter(IBreedService service, IRequestQueue queue, IBreedView view)
    {
        _service = service;
        _queue = queue;
        _view = view;

        // Subscribe to breed-click events and tie the subscription to the view's CompositeDisposable
        _view.OnBreedClicked
             .Subscribe(id => LoadBreedDetail(id));
        //.AddTo(_view.Disposables);  // Ensures automatic disposal when view is destroyed :contentReference[oaicite:2]{index=2}
    }
    public void OnEnter()
    {
        _queue.Enqueue(new FetchBreedsRequest(_service))
              .ContinueWith(breeds =>
              {
                  _view.UpdateBreedsList(breeds);
              }) // Chain continuation on success only :contentReference[oaicite:3]{index=3}
              .Forget();   // Fire-and-forget, no awaiting required :contentReference[oaicite:4]{index=4}
    }

    void LoadBreedDetail(string breedId)
    {
        _view.ShowItemLoader(breedId, true); // Show loading indicator for the item
        _queue.Cancel(r => r is FetchBreedDetailRequest); // Cancel any ongoing requests

        _queue.Enqueue(new FetchBreedDetailRequest(_service, breedId))
              .ContinueWith(detail =>
              {
                  _view.ShowItemLoader(breedId, false); // Hide loading indicator
                  if (detail != null) // Check if detail is not null
                  {
                      _view.ShowBreedPopup(detail); // Show the popup with the breed details
                  }
                  else
                  {
                      // Handle the case where detail is null (e.g., show an error message)
                  }
              })
              .Forget();
    }
    public void OnExit()
    {
        _queue.Cancel(r => r is FetchBreedsRequest || r is FetchBreedDetailRequest);
    }
}
