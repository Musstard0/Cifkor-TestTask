using Zenject;

public class GameInstaller : MonoInstaller
{

    public override void InstallBindings()
    {
        Container.Bind<IRequestQueue>().To<RequestQueue>().AsTransient();

        // Weather module bindings 
        Container.Bind<IWeatherService>().To<WeatherService>().AsTransient();
        Container.Bind<IWeatherView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<WeatherPresenter>().AsSingle();

        // Breed module bindings
        Container.Bind<IBreedService>().To<BreedService>().AsSingle();
        Container.Bind<IBreedView>().To<BreedView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BreedPresenter>().AsSingle();


    }
}