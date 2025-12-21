using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.UserTests;

[TestFixture]
public class UserTests
{
    private User user;
    private AiModel model;
    private Preset preset;

    [SetUp]
    public void Setup()
    {
        user = new User(1);
        model = new AiModel(Guid.NewGuid(), "id", "model_name", A.Fake<AiProvider>());
        preset = new Preset(Guid.NewGuid(), "preset_name", PromptSettings.Default);
    }

    [Test]
    public void AddPreset_Single()
    {
        user.AddPreset(preset);
        Assert.That(user.UserPresets.Contains(preset));
    }

    [Test]
    public void DeletePreset_Single()
    {
        user.AddPreset(preset);
        user.DeletePreset(preset);
        Assert.That(user.UserPresets.Contains(preset), Is.False);
    }

    [Test]
    public void ChosenPreset_IsNullInitially()
    {
        Assert.That(user.ChosenPreset, Is.Null);
    }

    [Test]
    public void ChosenPreset_PresetIsChosen()
    {
        user.AddPreset(preset);
        user.ChoosePreset(preset);
        Assert.That(user.ChosenPreset, Is.EqualTo(preset));
    }

    [Test]
    public void AddModel_Single()
    {
        user.AddModel(model);
        Assert.That(user.UserModels.Contains(model));
    }

    [Test]
    public void DeleteModel_Single()
    {
        user.AddModel(model);
        user.DeleteModel(model);
        Assert.That(user.UserModels.Contains(model), Is.False);
    }

    [Test]
    public void ChosenModel_IsNullInitially()
    {
        Assert.That(user.ChosenModel, Is.Null);
    }

    [Test]
    public void ChosenModel_ModelIsChosen()
    {
        user.AddModel(model);
        user.ChooseModel(model);
        Assert.That(user.ChosenModel, Is.EqualTo(model));
    }

    [Test]
    public void AddComparison_WithoutExceedingLimit()
    {
        user.AddModel(model);
        user.AddPreset(preset);

        var compareElem = new CompareElement(preset, model);
        var config = new CompareConfig([compareElem], compareElem, DateTime.Now);
        user.AddComparison(config, 100);
        var comparison = user.Comparisons.FirstOrDefault();

        Assert.That(comparison!.Elements, Is.EqualTo(config.Elements));
        Assert.That(comparison.Orchestrator, Is.EqualTo(config.Orchestrator));
        Assert.That(comparison.CreationTime, Is.EqualTo(config.CreationTime));
    }

    [Test]
    public void AddComparison_ExceedLimit()
    {
        user.AddModel(model);
        user.AddPreset(preset);

        var compareElem = new CompareElement(preset, model);
        var config = new CompareConfig([compareElem], compareElem, DateTime.Now);
        user.AddComparison(config, 0);
        Assert.That(user.Comparisons.Count, Is.EqualTo(0));
    }
}