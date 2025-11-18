namespace AICore.Entities;

public class User(long id) : Entity<long>(id)
{
    public UserStateMachine StateMachine { get; private set; } = new();

    public IReadOnlyCollection<AiModel> UserModels => Models;
    private HashSet<AiModel> Models { get; set; } = [];
    
    public IReadOnlyCollection<Preset> UserPresets => Presets;
    private HashSet<Preset> Presets { get; set; } = [];
    
    public void AddModel(AiModel model) => Models.Add(model);

    public void AddPreset(Preset preset) => Presets.Add(preset);

    public void DeletePreset(Preset preset)
    {
        Presets.Remove(preset);
    }
}