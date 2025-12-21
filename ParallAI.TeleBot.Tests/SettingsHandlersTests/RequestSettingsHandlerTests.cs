using ParallAI.Core.States;
using ParallAI.TeleBot.Settings;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

[TestFixture]
public class RequestSettingsHandlerTests : SettingsHandlerTests<RequestSettingsHandler, RequestSettingsState>
{
    private RequestSettingsState state;
    private Message message;
    private CallbackQuery query;

    protected override RequestSettingsHandler CreateHandler() => new();
    protected override RequestSettingsState CreateInitialState() => new();

    [SetUp]
    public void Setup()
    {
        state = CreateInitialState();
        message = new Message();
        query = new CallbackQuery { Message = message };
    }

    [Test]
    public async Task FinalizeSettings_CurrentStateIsRequest()
    {
        var handler = CreateHandler();
        User.StateMachine.Push(state);
        await handler.FinalizeSettings(state, query, Bot, User);
        Assert.That(User.StateMachine.Current, Is.TypeOf(typeof(RequestState)));
    }
}