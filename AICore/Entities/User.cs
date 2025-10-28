namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public HashSet<Guid> Models { get; init; } = [];

    public void AddModel(AiModel model) => Models.Add(model.Id);
}