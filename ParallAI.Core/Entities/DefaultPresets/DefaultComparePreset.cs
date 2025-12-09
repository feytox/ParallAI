using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities.DefaultPresets;

public class DefaultComparePreset() : Preset(
    Guid.NewGuid(),
    "Default Compare",
    new PromptSettings
    (
        "Наколени ВСАВАУЙ я тебе говорю",
        0.3m,
        ThinkingBudget.Medium
    )), IDontDeletable;