namespace ParallAI.Core.ValueTypes;

public record PromptSettings(string SystemPrompt, decimal Temperature, ThinkingBudget ThinkingBudget = ThinkingBudget.None)
{
    public static readonly PromptSettings Default = new("", 0.7m);

    public bool HasSystemInstruction => !string.IsNullOrEmpty(SystemPrompt);
}