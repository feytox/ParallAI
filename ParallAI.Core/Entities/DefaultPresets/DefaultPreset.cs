using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities.DefaultPresets;

public class DefaultPreset() : Preset(
    Guid.NewGuid(),
    "Default",
    new PromptSettings
    (
        "", // TODO: default prompt
        0.42m,
        ThinkingBudget.Low
    )), IImmutableElement;