using ParallAI.Core.Entities.DefaultPresets;
using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Entities;

public class User(long id) : Entity<long>(id)
{
    public UserStateMachine StateMachine { get; private set; } = new();

    public IReadOnlyList<AiModel> UserModels => Models;
    private List<AiModel> Models { get; set; } = [];

    public IReadOnlyList<Preset> UserPresets => Presets;
    private List<Preset> Presets { get; set; } = [new DefaultPreset(), new DefaultComparePreset()];

    public Preset? ChosenPreset => ChosenPresetId is null ? null : Presets.Find(preset => preset.Id == ChosenPresetId);
    private Guid? ChosenPresetId { get; set; }

    public AiModel? ChosenModel => ChosenModelId is null ? null : Models.Find(model => model.Id == ChosenModelId);
    private Guid? ChosenModelId { get; set; }

    public IReadOnlyList<CompareConfig> Comparisons => CompareConfigs.Select(MapComparison).ToList();
    public IEnumerable<DateTime> ComparisonDates => CompareConfigs.Select(dto => dto.CreationTime);
    private List<CompareConfigDto> CompareConfigs { get; set; } = [];

    public void AddModel(AiModel model) => Models.Add(model);

    public AiModel? GetModel(Guid id) => Models.FirstOrDefault(model => model.Id == id);

    public void DeleteModel(AiModel model)
    {
        Models.Remove(model);
        CompareConfigs.RemoveAll(dto => dto.Elements.Any(elementDto => elementDto.ModelId == model.Id));
    }

    public void ChooseModel(AiModel model) => ChosenModelId = model.Id;

    public void AddPreset(Preset preset) => Presets.Add(preset);

    public Preset? GetPreset(Guid id) => Presets.FirstOrDefault(preset => preset.Id == id);

    public void DeletePreset(Preset preset)
    {
        Presets.Remove(preset);
        CompareConfigs.RemoveAll(dto => dto.Elements.Any(elementDto => elementDto.PresetId == preset.Id));
    }

    public void ChoosePreset(Preset preset) => ChosenPresetId = preset.Id;

    public void AddComparison(CompareConfig config, int limit)
    {
        CompareConfigs.Add(config.MapToDto());
        if (CompareConfigs.Count > limit)
            CompareConfigs.RemoveAt(0);
    }

    private CompareConfig MapComparison(CompareConfigDto dto)
    {
        var elements = dto.Elements
            .Select(MapCompareElement)
            .ToArray();
        var orchestrator = MapCompareElement(dto.Orchestrator);
        
        return new CompareConfig(elements, orchestrator, dto.CreationTime);
    }

    private CompareElement MapCompareElement(CompareElementDto dto)
    {
        var preset = GetPreset(dto.PresetId)!;
        var model = GetModel(dto.ModelId)!;
        return new CompareElement(preset, model);
    }
}