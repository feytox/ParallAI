namespace AICore.Entities;

public class AiModel(Guid id, string name) : Entity<Guid>(id)
{
    public string Name { get; private set; } = name;
}