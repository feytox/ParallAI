using System.Collections.Immutable;

namespace AICore.States;

public class PresetState : UserState
{
    protected override ImmutableArray<UserStateType> steps { get; } =
    [
        UserStateType.PresetWaitName,
        UserStateType.PresetWaitModel,
        UserStateType.PresetWaitPrompt,
        UserStateType.PresetWaitTemperature
    ];

    public string? Name { get; set; }
    public string? Model { get; set; }
    public string? Prompt { get; set; }
    public float Temperature { get; set; }
}
