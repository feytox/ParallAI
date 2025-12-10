using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities.DefaultPresets;

public class DefaultPreset() : Preset(
    Guid.NewGuid(),
    "Default",
    new PromptSettings
    (
        "Представь что ты Саша Алабастер и реши задачу",
        0.42m,
        ThinkingBudget.Low
    )), IImmutableElement;