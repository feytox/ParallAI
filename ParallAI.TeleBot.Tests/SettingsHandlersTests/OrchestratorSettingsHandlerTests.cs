using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

public class OrchestratorSettingsHandlerTests
    : SettingsHandlerTests<OrchestratorSettingsHandler, OrchestratorSettingsState>
{
    protected override OrchestratorSettingsHandler CreateHandler() => new();

    protected override OrchestratorSettingsState CreateInitialState() => new([]);

    [Test]
    public async Task FinalizeSettings_CurrentStateIsCompare()
    {
        var handler = CreateHandler();
        var query = new CallbackQuery();
        var state = CreateInitialState();

        var preset = A.Fake<Preset>();
        var model = A.Fake<AiModel>();
        state.Preset = preset;
        state.Model = model;

        User.StateMachine.Push(state);

        await handler.FinalizeSettings(state, query, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(CompareState)));
        Assert.That(User.Comparisons.Count, Is.EqualTo(1));
    }
}