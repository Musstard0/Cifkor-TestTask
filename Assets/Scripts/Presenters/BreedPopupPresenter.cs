
public class BreedPopupPresenter
{
    public static BreedPopupPresenter Instance { get; private set; }
    readonly IBreedPopupView view;

    public BreedPopupPresenter(IBreedPopupView view)
    {
        Instance = this;
        this.view = view;
    }
    public void Show(BreedModel breed)
    {
        view.SetData(breed.Name, breed.Description);
        view.Show();
    }
}
