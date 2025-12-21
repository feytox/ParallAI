namespace ParallAI.Core.ValueTypes;

public record PromptSettings(string SystemPrompt, decimal Temperature, ThinkingBudget ThinkingBudget)
{
    public static readonly PromptSettings Default = new("", 0.7m, ThinkingBudget.None);

    public bool HasSystemInstruction => !string.IsNullOrEmpty(SystemPrompt);
}