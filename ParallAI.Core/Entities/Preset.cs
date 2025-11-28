using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public class Preset(
    Guid id,
    string name,
    PromptSettings promptSettings) : Entity<Guid>(id)
{
    public string Name { get; set; } = name;

    public PromptSettings PromptSettings { get; set; } = promptSettings;
}