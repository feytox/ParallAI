using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public class Preset(
    Guid id,
    string name,
    PromptSettings promptSettings) : Entity<Guid>(id)
{
    public string Name { get; private set; } = name;

    public PromptSettings PromptSettings { get; private set; } = promptSettings;
}