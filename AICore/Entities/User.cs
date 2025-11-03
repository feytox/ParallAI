namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public IReadOnlyCollection<AiModel> UserModels => Models;

    private HashSet<AiModel> Models { get; set; } = [];
    
    public void AddModel(AiModel model) => Models.Add(model);
}