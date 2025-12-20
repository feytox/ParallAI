using FakeItEasy;
using FluentAssertions;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class CompareSettingsStateTests
{
    private CompareSettingsState state;
    private Preset preset;
    private AiModel model;

    [SetUp]
    public void Setup()
    {
        state = new CompareSettingsState();
        preset = A.Fake<Preset>();
        model = A.Fake<AiModel>();
    }
    
    [Test]
    public void AddElement_Single()
    {
        var configElem = new CompareElement(preset, model);
        state.AddElement(configElem);
        Assert.That(state.ConfiguredElements.Contains(configElem));
    }
    
    [Test]
    public void RemoveAt_Single()
    {
        var configElem = new CompareElement(preset, model);
        state.AddElement(configElem);
        state.RemoveAt(0);
        Assert.That(state.ConfiguredElements.Contains(configElem), Is.False);
    }

    [Test]
    public void RemoveAt_ArgumentOutOfRange()
    {
        Assert.Catch<ArgumentOutOfRangeException>(() => state.RemoveAt(0));
    }

    [Test]
    public void ToOrchestratorState_MoreThanZeroElems()
    {
        var configElem = new CompareElement(preset, model);
        state.AddElement(configElem);
        state.AddElement(configElem);
        var result = state.ToOrchestratorState();
        var expected = new OrchestratorSettingsState(state.ConfiguredElements.ToArray());
        result.Should().BeEquivalentTo(expected);
    }
    
    [Test]
    public void ToOrchestratorState_ZeroElems()
    {
        var result = state.ToOrchestratorState();
        var expected = new OrchestratorSettingsState(state.ConfiguredElements.ToArray());
        result.Should().BeEquivalentTo(expected);
    }
}