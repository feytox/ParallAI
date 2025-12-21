using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.States;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

[TestFixture]
public class ModelSettingsHandlerTests : SettingsHandlerTests<ModelSettingsHandler, ModelSettingsState>
{
    private Message message;
    private CallbackQuery query;

    protected override ModelSettingsHandler CreateHandler() => new();

    protected override ModelSettingsState CreateInitialState() => new(Guid.NewGuid());

    [SetUp]
    public void Setup()
    {
        message = new Message();
        query = new CallbackQuery { Message = message };
    }

    [Test]
    public async Task FinalizeSettings_AddNewModel()
    {
        var state = CreateInitialState();
        var handler = CreateHandler();

        User.StateMachine.Push(state);

        await handler.FinalizeSettings(state, query, Bot, User);
        var modelsCount = User.UserModels.Count;
        Assert.That(modelsCount, Is.EqualTo(1));
    }

    [Test]
    public async Task FinalizeSettings_ChangeModel()
    {
        var guid = Guid.NewGuid();
        var state = new ModelSettingsState(guid);
        var msg = new Message();
        var callbackQuery = new CallbackQuery { Message = msg };
        var handler = CreateHandler();

        var model = new AiModel(guid, "ID", "name", new GeminiProvider("token"));
        User.AddModel(model);
        User.StateMachine.Push(state);

        var newModelId = "new ID";
        var newDisplayName = "new name";
        var newProvider = new GeminiProvider("new token");

        state.ModelId = newModelId;
        state.DisplayName = newDisplayName;
        state.Provider = newProvider;

        await handler.FinalizeSettings(state, callbackQuery, Bot, User);

        var modelsCount = User.UserModels.Count;
        var expectedModel = new AiModel(guid, newModelId, newDisplayName, newProvider);

        Assert.That(modelsCount, Is.EqualTo(1));
        Assert.That(User.GetModel(guid), Is.EqualTo(expectedModel));
    }
}