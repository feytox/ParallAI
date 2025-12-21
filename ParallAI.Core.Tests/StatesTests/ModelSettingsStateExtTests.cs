using FakeItEasy;
using FluentAssertions;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class ModelSettingsStateExtTests
{
    private Guid guid;

    [SetUp]
    public void Setup()
    {
        guid = Guid.NewGuid();
    }

    [Test]
    public void ToModel_WithDisplayName()
    {
        var state = new ModelSettingsState(guid)
        {
            DisplayName = "Model name",
            ModelId = "model id",
            Provider = A.Fake<AiProvider>()
        };

        var result = state.ToModel();
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, !Is.EqualTo(state.Id));
            Assert.That(result.DisplayName, Is.EqualTo(state.DisplayName));
            Assert.That(result.ModelId, Is.EqualTo(state.ModelId));
            Assert.That(result.Provider, Is.EqualTo(state.Provider));
        });
    }

    [Test]
    public void ToModel_WithoutDisplayName()
    {
        var state = new ModelSettingsState(guid)
        {
            ModelId = "model id",
            Provider = A.Fake<AiProvider>()
        };

        var result = state.ToModel();
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, !Is.EqualTo(state.Id));
            Assert.That(result.DisplayName, Is.EqualTo(state.ModelId));
            Assert.That(result.ModelId, Is.EqualTo(state.ModelId));
            Assert.That(result.Provider, Is.EqualTo(state.Provider));
        });
    }

    [TestCase("id", "name")]
    [TestCase(null, null)]
    public void ToState(string? modelId, string? displayName)
    {
        var provider = A.Fake<AiProvider>();
        var model = new AiModel(guid, modelId!, displayName!, provider);
        var result = model.ToState();

        var expected = new ModelSettingsState(guid)
        {
            ModelId = model.ModelId,
            DisplayName = model.DisplayName,
            Provider = provider
        };
        result.Should().BeEquivalentTo(expected);
    }

    [TestCase("id", "name")]
    [TestCase(null, null)]
    public void ApplyChanges_FieldsIsEquals(string? modelId, string? displayName)
    {
        var stateGuid = Guid.NewGuid();
        var modelGuid = Guid.NewGuid();
        var state = new ModelSettingsState(stateGuid)
        {
            ModelId = modelId,
            DisplayName = displayName,
            Provider = A.Fake<AiProvider>()
        };

        var model = new AiModel(modelGuid, "", "", null);
        state.ApplyChanges(model);

        Assert.Multiple(() =>
        {
            Assert.That(model.Id, !Is.EqualTo(state.Id));
            Assert.That(model.ModelId, Is.EqualTo(state.ModelId));
            Assert.That(model.DisplayName, Is.EqualTo(state.DisplayName));
            Assert.That(model.Provider, Is.EqualTo(state.Provider));
        });
    }
}