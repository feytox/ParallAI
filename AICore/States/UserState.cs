using System.Text.Json.Serialization;

namespace AICore.States;

[JsonDerivedType(typeof(DefaultUserState), (int)UserStateType.Default)]
[JsonDerivedType(typeof(AddPresetState), (int)UserStateType.PresetCreation)]
public abstract class UserState
{
    public abstract UserStateType StateType { get; }

    public string? NextCommandName { get; set; }
}