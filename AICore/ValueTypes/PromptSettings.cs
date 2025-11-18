namespace AICore.ValueTypes;

// TODO: add thinking budget
public record PromptSettings(string SystemPrompt, decimal Temperature, int ThinkingBudget)
{
    public static readonly PromptSettings Default = new("", new decimal(0.7), 2048);

    public bool HasSystemInstruction => !string.IsNullOrEmpty(SystemPrompt);
}