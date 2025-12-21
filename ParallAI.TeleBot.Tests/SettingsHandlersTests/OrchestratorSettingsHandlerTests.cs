using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

public class OrchestratorSettingsHandlerTests 
    : SettingsHandlerTests<OrchestratorSettingsHandler, OrchestratorSettingsState>
{
    protected override OrchestratorSettingsHandler CreateHandler() => new();

    protected override OrchestratorSettingsState CreateInitialState() => new([]);

    [Test]
    public async Task FinalizeSettings()
    {
        var handler = CreateHandler();
        var query = new CallbackQuery();
        var state = CreateInitialState();

        var preset = new Preset(Guid.NewGuid(), "name", PromptSettings.Default);
        var model = new AiModel(Guid.NewGuid(), "ID", "name", new GeminiProvider("token"));
        state.Preset = preset;
        state.Model = model;

        User.StateMachine.Push(state);

        await handler.FinalizeSettings(state, query, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(CompareState)));
        Assert.That(User.Comparisons.Count, Is.EqualTo(1));
    }
}