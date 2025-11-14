using AICore.States;

namespace TeleBot.Example.States;

public enum PresetStep
{
    Name,
    Model,
    Prompt,
    Temperature
}

public class PresetState() : SequentialState<PresetStep>(Enum.GetValues<PresetStep>())
{
    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? Prompt { get; set; }
    public float Temperature { get; set; }
}