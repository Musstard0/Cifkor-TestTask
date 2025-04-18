using System;
using System.Collections.Generic;
using UniRx;

public interface IBreedView
{
    IObservable<string> OnBreedClicked { get; }
    void UpdateBreedsList(List<BreedModel> breeds);
    void ShowItemLoader(string breedId, bool active);
    void ShowBreedPopup(BreedModel breedDetail);
}
