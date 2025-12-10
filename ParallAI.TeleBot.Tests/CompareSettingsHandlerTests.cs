using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Settings;


namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class CompareSettingsHandlerTests : SettingsHandlerTests<CompareSettingsHandler, CompareSettingsState>
{
    protected override CompareSettingsHandler CreateHandler()
    {
        return new CompareSettingsHandler();
    }

    [Test]
    public void RemoveCompareElement()
    {
        var handler = CreateHandler();
        var state = new CompareSettingsState();
        var fakePreset = A.Fake<Preset>();
        var fakeModel = A.Fake<AiModel>();
        state.ConfiguredElements.Add(new CompareElement(fakePreset, fakeModel));
        state.ConfiguredElements.Add(new CompareElement(fakePreset, fakeModel));
        handler.RemoveCompareElement(state, 0);
        
        Assert.That(state.Reactivated);
        Assert.That(state.ConfiguredElements.Count == 1);
    }
    
    [Test]
    public async Task SaveSettingsToUser()
    {
        var handler = CreateHandler();
        var state = new CompareSettingsState();
        User.StateMachine.Push(state);
        await handler.FinalizeSettings(state, ChatId, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(OrchestratorSettingsState)));
    }
}