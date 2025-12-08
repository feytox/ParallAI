using ParallAI.Core.Entities;

namespace ParallAI.Core.ValueTypes;

public enum RequestMode
{
    Single,
    Continuous
}

public record RequestConfig(AiModel Model, Preset? Preset, RequestMode RequestMode);