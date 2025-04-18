public class BreedSelectedEvent
{
    public BreedModel Detail { get; }
    public BreedSelectedEvent(BreedModel detail) => Detail = detail;
}
