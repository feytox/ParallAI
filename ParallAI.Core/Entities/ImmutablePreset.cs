using ParallAI.Core.Util;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public class ImmutablePreset(Guid id, string name, PromptSettings promptSettings)
    : Preset(id, name, promptSettings), IImmutableElement;