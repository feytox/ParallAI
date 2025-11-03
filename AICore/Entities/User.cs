namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public UserStateMachine StateMachine { get; private set; } = new();

    public IReadOnlyCollection<AiModel> UserModels => Models;

    private HashSet<AiModel> Models { get; set; } = [];
    
    public void AddModel(AiModel model) => Models.Add(model);
}