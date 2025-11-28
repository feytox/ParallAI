using ParallAI.Core.States.Common;

namespace ParallAI.Core.Entities;

public class User(long id) : Entity<long>(id)
{
    public UserStateMachine StateMachine { get; private set; } = new();

    public IReadOnlyList<AiModel> UserModels => Models;
    private List<AiModel> Models { get; set; } = [];
    
    public IReadOnlyList<Preset> UserPresets => Presets;
    private List<Preset> Presets { get; set; } = [];

    public Preset? ChosenPreset => ChosenPresetId is null ? null : Presets.Find(preset => preset.Id == ChosenPresetId);
    private Guid? ChosenPresetId { get; set; }
    
    public void AddModel(AiModel model) => Models.Add(model);

    public void AddPreset(Preset preset) => Presets.Add(preset);

    public void DeletePreset(Preset preset)
    {
        Presets.Remove(preset);
    }

    public void ChoosePreset(Preset preset)
    {
        ChosenPresetId = preset.Id;
    }
}