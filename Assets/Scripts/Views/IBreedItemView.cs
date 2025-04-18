using System;

public interface IBreedItemView
{
    void SetBreedName(string name);
    void SetOnClick(Action onClick);
}