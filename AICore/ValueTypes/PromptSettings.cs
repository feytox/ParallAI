namespace AICore.ValueTypes;

// TODO: add thinking budget
public record PromptSettings(string SystemInstructions, double Temperature, int ThinkingBudget)
{
    public static readonly PromptSettings Default = new("", 0.7d, 2048);

    public bool HasSystemInstruction => !string.IsNullOrEmpty(SystemInstructions);
}