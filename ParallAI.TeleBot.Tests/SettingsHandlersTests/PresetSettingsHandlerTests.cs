using ParallAI.Core.Entities;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

public class PresetSettingsHandlerTests : SettingsHandlerTests<PresetSettingsHandler, PresetSettingsState>
{
    private PresetSettingsHandler handler;
    private Message message;
    private CallbackQuery query;

    protected override PresetSettingsHandler CreateHandler() => new();

    protected override PresetSettingsState CreateInitialState() => new(Guid.NewGuid());

    [SetUp]
    public void Setup()
    {
        handler = CreateHandler();
        message = new Message();
        query = new CallbackQuery { Message = message };
    }

    [Test]
    public async Task FinalizeSettings_AddNewPreset()
    {
        var state = CreateInitialState();

        state.Name = "name";
        state.SystemPrompt = "prompt";
        state.Temperature = 1;
        state.ThinkingBudget = ThinkingBudget.None;

        User.StateMachine.Push(state);

        await handler.FinalizeSettings(state, query, Bot, User);
        var presetsCount = User.UserPresets.Count;
        Assert.That(presetsCount, Is.EqualTo(3));
    }

    [Test]
    public async Task FinalizeSettings_ChangePreset()
    {
        var guid = Guid.NewGuid();
        var state = new PresetSettingsState(guid);

        var preset = new Preset(guid, "name", PromptSettings.Default);
        User.AddPreset(preset);
        User.StateMachine.Push(state);

        var newPresetName = "new name";
        var newPromptSettings = new PromptSettings("Системный промпт", 1, ThinkingBudget.Dynamic);

        state.Name = newPresetName;
        state.SystemPrompt = newPromptSettings.SystemPrompt;
        state.Temperature = newPromptSettings.Temperature;
        state.ThinkingBudget = newPromptSettings.ThinkingBudget;

        await handler.FinalizeSettings(state, query, Bot, User);

        var presetsCount = User.UserPresets.Count;
        var expectedPreset = new Preset(guid, newPresetName, newPromptSettings);

        Assert.That(presetsCount, Is.EqualTo(3));
        Assert.That(User.GetPreset(guid), Is.EqualTo(expectedPreset));
    }
}