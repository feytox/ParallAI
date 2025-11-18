namespace AICore.States;

public enum PresetStep
{
    Name,
    SystemPrompt,
    Temperature,
    ThinkingBudget
}

public class PresetState() : SequentialState<PresetStep>(Enum.GetValues<PresetStep>())
{
    public string? Name { get; set; }
    public string? SystemPrompt { get; set; }
    public float Temperature { get; set; }
    
    public int ThinkingBudget { get; set; }
}