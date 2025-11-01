using AICore.ValueTypes;

namespace AICore.Entities;

public class AiModel(Guid id, string modelId, string displayName, AiProvider provider) : Entity<Guid>(id)
{
    public string ModelId { get; private set; } = modelId;
    public string DisplayName { get; private set; } = displayName;
    public AiProvider Provider { get; private set; } = provider;
}