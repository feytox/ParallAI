using AICore.States;

namespace AICore.Entities;


public class User(long id) : Entity<long>(id)
{
    public HashSet<AiModel> Models { get; init; } = [];
    public UserStateMachine StateMachine { get; private set; } = new();

    public void AddModel(AiModel model) => Models.Add(model);
}
