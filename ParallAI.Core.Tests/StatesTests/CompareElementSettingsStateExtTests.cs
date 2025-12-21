using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class CompareElementSettingsStateExtTests
{
    private CompareElementSettingsState state;

    [SetUp]
    public void Setup()
    {
        state = new CompareElementSettingsState();
    }

    [Test]
    public void ToTextLines_StateElemsIsNotNull()
    {
        state.Model = A.Fake<AiModel>();
        state.Preset = A.Fake<Preset>();
        var textLines = state.ToTextLines();

        var expectedModel = $"Модель: {state.Model.DisplayName}";
        var expectedPreset = $"Пресет: {state.Preset.Name}";
        Assert.That(textLines, Is.EquivalentTo(new[] { expectedPreset, expectedModel }));
    }

    [Test]
    public void ToTextLines_OneStateElemIsNull()
    {
        state.Model = A.Fake<AiModel>();
        var textLines = state.ToTextLines();

        var expectedModel = $"Модель: {state.Model.DisplayName}";
        Assert.That(textLines, Is.EquivalentTo(new[] { expectedModel }));
    }

    [Test]
    public void ToTextLines_StateElemsIsNull()
    {
        var textLines = state.ToTextLines();
        Assert.That(textLines, Is.EquivalentTo(Array.Empty<string>()));
    }

    [Test]
    public void ToElement_StateElemsIsNotNull()
    {
        state.Model = A.Fake<AiModel>();
        state.Preset = A.Fake<Preset>();
        var result = state.ToElement();

        var expected = new CompareElement(state.Preset, state.Model);
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void ToElement_StateElemsIsNull()
    {
        var result = state.ToElement();
        Assert.That(result.Model, Is.Null);
        Assert.That(result.Preset, Is.Null);
    }
}