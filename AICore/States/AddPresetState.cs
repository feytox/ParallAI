namespace AICore.States;

public class AddPresetState : UserState
{
    public override UserStateType StateType => UserStateType.PresetCreation;
    
    public string? Prompt { get; set; }
    public float Temperature { get; set; }
}