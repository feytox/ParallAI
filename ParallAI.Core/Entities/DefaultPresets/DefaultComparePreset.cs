using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities.DefaultPresets;

public class DefaultComparePreset() : Preset(
    Guid.NewGuid(),
    "Default Compare",
    new PromptSettings
    (
        "", // TODO: default compare prompt
        0.3m,
        ThinkingBudget.Medium
    )), IImmutableElement;