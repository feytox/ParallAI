namespace ParallAI.Core.ValueTypes;

public record PromptSettings(string SystemPrompt, decimal Temperature, ThinkingBudget? ThinkingBudget = null)
{
    public static readonly PromptSettings Default = new("", new decimal(0.7));

    public bool HasSystemInstruction => !string.IsNullOrEmpty(SystemPrompt);
}