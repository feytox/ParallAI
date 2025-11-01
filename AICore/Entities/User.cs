namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public HashSet<AiModel> Models { get; init; } = [];
    public void AddModel(AiModel model) => Models.Add(model);
}