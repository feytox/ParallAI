namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public IReadOnlyCollection<AiModel> Models => models;

    private HashSet<AiModel> models { get; set; } = [];
    
    public void AddModel(AiModel model) => models.Add(model);
}