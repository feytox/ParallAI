using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public class AiModel(Guid id, string modelId, string displayName, AiProvider provider) : Entity<Guid>(id)
{
    public string ModelId { get; set; } = modelId;
    public string DisplayName { get; set; } = displayName;
    public AiProvider Provider { get; set; } = provider;
}