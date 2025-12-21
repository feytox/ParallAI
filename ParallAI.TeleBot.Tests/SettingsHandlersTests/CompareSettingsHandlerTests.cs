using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

[TestFixture]
public class CompareSettingsHandlerTests : SettingsHandlerTests<CompareSettingsHandler, CompareSettingsState>
{
    private CompareSettingsState state;
    private Message message;
    private CallbackQuery query;
    private CompareSettingsHandler handler;

    protected override CompareSettingsHandler CreateHandler() => new();
    protected override CompareSettingsState CreateInitialState() => new();

    [SetUp]
    public void Setup()
    {
        state = CreateInitialState();
        message = new Message();
        query = new CallbackQuery { Message = message };
        handler = CreateHandler();
    }

    [Test]
    public void RemoveCompareElement()
    {
        var preset = A.Fake<Preset>();
        var model = A.Fake<AiModel>();
        state.ConfiguredElements.Add(new CompareElement(preset, model));
        state.ConfiguredElements.Add(new CompareElement(preset, model));
        handler.RemoveCompareElement(state, 0);

        Assert.That(state.Reactivated);
        Assert.That(state.ConfiguredElements.Count == 1);
    }

    [Test]
    public async Task FinalizeSettings_CurrentStateIsOrchestratorSettings()
    {
        User.StateMachine.Push(state);
        await handler.FinalizeSettings(state, query, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(OrchestratorSettingsState)));
    }
}