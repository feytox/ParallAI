namespace AICore.Entities;

public class Preset(
    Guid id,
    string name,
    string systemPrompt,
    float temperature,
    int thinkingBudget) : Entity<Guid>(id)
{
    public string Name { get; private set; } = name;

    public string SystemPrompt { get; private set; } = systemPrompt;

    public float Temperature { get; private set; } = temperature;

    public int ThinkingBudget { get; private set; } = thinkingBudget;
}