// Presenters/BreedPresenter.cs
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TabController : MonoBehaviour
{
    [Header("Panel References")]
    [SerializeField] private GameObject weatherPanel;  // Panel containing WeatherView and its UI
    [SerializeField] private GameObject breedsPanel;   // Panel containing BreedView and its UI

    [Header("Tab Buttons")]
    [SerializeField] private Button weatherTabButton;
    [SerializeField] private Button breedsTabButton;

    private WeatherPresenter _weatherPresenter;
    private BreedPresenter _breedPresenter;

    // Zenject Injection of the presenters
    [Inject]
    public void Construct(WeatherPresenter weatherPresenter, BreedPresenter breedPresenter)
    {
        _weatherPresenter = weatherPresenter;
        _breedPresenter = breedPresenter;
    }

    private void Start()
    {
        // Set initial tab (e.g., Weather)
        ShowWeatherTab();

        // Hook up button events
        weatherTabButton.onClick.AddListener(ShowWeatherTab);
        breedsTabButton.onClick.AddListener(ShowBreedsTab);
    }

    private void ShowWeatherTab()
    {
        // If switching from breeds, tell its presenter to stop processing.
        _breedPresenter?.OnExit();

        // Activate the Weather panel and deactivate the Breeds panel.
        weatherPanel.SetActive(true);
        breedsPanel.SetActive(false);

        // Notify WeatherPresenter that the tab is active.
        _weatherPresenter?.OnEnter();
    }

    private void ShowBreedsTab()
    {
        // If switching from weather, tell its presenter to stop processing.
        _weatherPresenter?.OnExit();

        // Activate the Breeds panel and deactivate the Weather panel.
        breedsPanel.SetActive(true);
        weatherPanel.SetActive(false);

        // Notify BreedPresenter that the tab is active.
        _breedPresenter?.OnEnter();
    }
}
