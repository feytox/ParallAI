using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;


namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class CompareSettingsHandlerTests : SettingsHandlerTests<CompareSettingsHandler, CompareSettingsState>
{
    private CompareSettingsState state;
    private Message message;
    private CallbackQuery query;
    
    protected override CompareSettingsHandler CreateHandler() => new();
    protected override CompareSettingsState CreateInitialState() => new();

    [SetUp]
    public void Setup()
    {
        state = CreateInitialState();
        message = new Message();
        query = new CallbackQuery { Message = message };
    }
    
    [Test]
    public void RemoveCompareElement()
    {
        var handler = CreateHandler();
        var fakePreset = A.Fake<Preset>();
        var fakeModel = A.Fake<AiModel>();
        state.ConfiguredElements.Add(new CompareElement(fakePreset, fakeModel));
        state.ConfiguredElements.Add(new CompareElement(fakePreset, fakeModel));
        handler.RemoveCompareElement(state, 0);
        
        Assert.That(state.Reactivated);
        Assert.That(state.ConfiguredElements.Count == 1);
    }
    
    [Test]
    public async Task FinalizeSettings()
    {
        var handler = CreateHandler();
        User.StateMachine.Push(state);
        await handler.FinalizeSettings(state, query, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(OrchestratorSettingsState)));
    }
}