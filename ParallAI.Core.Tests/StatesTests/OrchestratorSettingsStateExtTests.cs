using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class OrchestratorSettingsStateExtTests
{
    [Test]
    public void Build()
    {
        var preset = A.Fake<Preset>();
        var model = A.Fake<AiModel>();
        var compareElem = new CompareElement(preset, model);
        var configElems = new[] { compareElem, compareElem, compareElem };
        var state = new OrchestratorSettingsState(configElems)
        {
            Preset = preset,
            Model = model
        };
        
        var result = state.Build();
        Assert.Multiple(() =>
        {
            Assert.That(result.Elements, Is.EquivalentTo(configElems));
            Assert.That(result.Orchestrator.Model, Is.EqualTo(model));
            Assert.That(result.Orchestrator.Preset, Is.EqualTo(preset));
        });
    }
}