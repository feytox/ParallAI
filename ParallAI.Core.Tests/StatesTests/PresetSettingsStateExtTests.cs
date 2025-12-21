using FluentAssertions;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class PresetSettingsStateExtTests
{
    private Guid guid;

    [SetUp]
    public void Setup()
    {
        guid = Guid.NewGuid();
    }

    [TestCase("name", "prompt", 1.0, ThinkingBudget.High)]
    [TestCase(null, null, 1.0, ThinkingBudget.None)]
    public void ToPreset(string? name, string? sysPrompt, decimal? temperature, ThinkingBudget budget)
    {
        var state = new PresetSettingsState(guid)
        {
            Name = name,
            SystemPrompt = sysPrompt,
            Temperature = temperature,
            ThinkingBudget = budget
        };

        var result = state.ToPreset();
        var expectedPromptSettings = new PromptSettings(sysPrompt!, temperature!.Value, budget);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, !Is.EqualTo(guid));
            Assert.That(result.Name, Is.EqualTo(state.Name));
            result.PromptSettings.Should().BeEquivalentTo(expectedPromptSettings);
        });
    }

    [Test]
    public void ToState()
    {
        var name = "name";
        var preset = new Preset(guid, name, PromptSettings.Default);

        var result = preset.ToState();
        var expected = new PresetSettingsState(guid)
        {
            Name = name,
            SystemPrompt = PromptSettings.Default.SystemPrompt,
            Temperature = PromptSettings.Default.Temperature,
            ThinkingBudget = PromptSettings.Default.ThinkingBudget
        };
        result.Should().BeEquivalentTo(expected);
    }

    [TestCase("name", "prompt", 1.0, ThinkingBudget.Medium)]
    [TestCase(null, "", 0.5, ThinkingBudget.None)]
    public void ApplyChanges(string? name, string sysPrompt, decimal temperature, ThinkingBudget budget)
    {
        var state = new PresetSettingsState(guid)
        {
            Name = name,
            SystemPrompt = sysPrompt,
            Temperature = temperature,
            ThinkingBudget = budget
        };

        var presetGuid = Guid.NewGuid();
        var preset = new Preset(presetGuid, "", PromptSettings.Default);
        state.ApplyChanges(preset);
        var expected = new Preset(presetGuid, state.Name!,
            new PromptSettings(state.SystemPrompt, state.Temperature.Value, state.ThinkingBudget));
        preset.Should().BeEquivalentTo(expected);
    }
}