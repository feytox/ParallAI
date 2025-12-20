using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class RequestSettingsStateExtTests
{
    private RequestSettingsState state;

    [SetUp]
    public void Setup()
    {
        state = new RequestSettingsState();
    }
    
    [Test]
    public void ToTextLines_StateElemsIsNotNull()
    {
        state.Model = A.Fake<AiModel>();
        state.Preset = A.Fake<Preset>();
        var textLines = state.ToTextLines();
        
        var expectedModel = $"Модель: {state.Model.DisplayName}";
        var expectedPreset = $"Пресет: {state.Preset.Name}";
        Assert.That(textLines, Is.EquivalentTo(new[] {expectedPreset, expectedModel} ));
    }

    [Test]
    public void ToTextLines_OneStateElemIsNull()
    {
        state.Model = A.Fake<AiModel>();
        var textLines = state.ToTextLines();
        
        var expectedModel = $"Модель: {state.Model.DisplayName}";
        Assert.That(textLines, Is.EquivalentTo(new[] {expectedModel} ));
    }
    
    [Test]
    public void ToTextLines_StateElemsIsNull()
    {
        var textLines = state.ToTextLines();
        Assert.That(textLines, Is.EquivalentTo(Array.Empty<string>()));
    }
}