using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public static class DefaultPresets
{
    private static readonly ImmutablePreset Default = new(new Guid("00000000-0000-0000-0000-000000000001"), 
        "Default", PromptSettings.Default);

    private static readonly ImmutablePreset DefaultCompare = new(new Guid("00000000-0000-0000-0000-000000000002"),
        "Default Compare", new PromptSettings(DefaultComparePrompt, 0.3m, ThinkingBudget.Medium));
    
    private const string DefaultComparePrompt = "Сравни приведённые ответы"; // TODO: default compare prompt (#63)
    
    public static readonly ImmutablePreset[] Presets = [Default, DefaultCompare];
}