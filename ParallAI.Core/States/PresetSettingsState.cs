using ParallAI.Core.Entities;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class PresetSettingsState(Guid? presetId) : SettingsState
{
    public Guid? PresetId { get; private set; } = presetId;
    public string? Name { get; set; }
    public string? SystemPrompt { get; set; }
    public decimal? Temperature { get; set; }
    public ThinkingBudget ThinkingBudget { get; set; } = ThinkingBudget.None;
}

public static class PresetSettingsExtensions
{
    public static Preset ToPreset(this PresetSettingsState state)
    {
        return new Preset(Guid.NewGuid(), state.Name!, state.CreateSettings());
    }

    public static PresetSettingsState ToState(this Preset preset)
    {
        return new PresetSettingsState(preset.Id)
        {
            Name = preset.Name,
            SystemPrompt = preset.PromptSettings.SystemPrompt,
            Temperature = preset.PromptSettings.Temperature,
            ThinkingBudget = preset.PromptSettings.ThinkingBudget
        };
    }

    public static void ApplyChanges(this PresetSettingsState state, Preset preset)
    {
        preset.Name = state.Name!;
        preset.PromptSettings = state.CreateSettings();
    }

    private static PromptSettings CreateSettings(this PresetSettingsState state)
    {
        return new PromptSettings(state.SystemPrompt!, state.Temperature!.Value, state.ThinkingBudget);
    } 
}