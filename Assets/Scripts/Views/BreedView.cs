using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public class BreedView : MonoBehaviour, IBreedView
{
    [SerializeField] private Transform listContainer;
    [SerializeField] private GameObject breedItemPrefab;
    [SerializeField] private BreedPopupView popup;

    private readonly Subject<string> _breedClickedSubject = new Subject<string>();
    public IObservable<string> OnBreedClicked => _breedClickedSubject.AsObservable();

    public CompositeDisposable Disposables { get; } = new CompositeDisposable();

    public void UpdateBreedsList(List<BreedModel> breeds)
    {
        foreach (Transform c in listContainer) Destroy(c.gameObject);

        foreach (var kv in breeds.Select((b, i) => (breed: b, index: i)))
        {
            var itemGO = Instantiate(breedItemPrefab, listContainer);
            var iv = itemGO.GetComponent<BreedItemView>();
            iv.SetData(kv.index + 1, kv.breed);
            iv.SetOnClick(() => _breedClickedSubject.OnNext(kv.breed.Id));
        }
    }

    public void ShowItemLoader(string breedId, bool active)
    {
        foreach (Transform child in listContainer)
        {
            var iv = child.GetComponent<BreedItemView>();
            if (iv != null && iv.BreedId == breedId)
            {
                iv.SetLoading(active);
                break;
            }
        }
    }
    public event Action<string> BreedClicked;

    public void OnBreedButtonClick(string breedId)
    {
        BreedClicked?.Invoke(breedId);
    }

    public void ShowBreedPopup(BreedModel breedDetail)
    {
        if (popup != null)
        {
            popup.Show();
            popup.SetData(breedDetail);
        }
    }
}
