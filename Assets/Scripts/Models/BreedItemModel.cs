
public class BreedItemModel
{
    public string Id { get; }
    public string Name { get; }
    public int Index { get; }
    public BreedItemModel(string id, string name, int index)
    {
        Id = id; Name = name; Index = index;
    }
}
