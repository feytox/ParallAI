namespace ParallAI.Core.ValueTypes;

public enum ThinkingBudget
{
    Unknown = 0,
    Dynamic,
    Minimal,
    Low,
    Medium,
    High
}

public static class ThinkingBudgetExt
{
    public static int ToThinkingTokens(this ThinkingBudget budget)
    {
        return budget switch
        {
            ThinkingBudget.Dynamic => -1,
            ThinkingBudget.Minimal => 1024,
            ThinkingBudget.Low => 1024,
            ThinkingBudget.Medium => 8192,
            ThinkingBudget.High => 24576,
            _ => throw new ArgumentOutOfRangeException(nameof(budget), budget, null)
        };
    }
}