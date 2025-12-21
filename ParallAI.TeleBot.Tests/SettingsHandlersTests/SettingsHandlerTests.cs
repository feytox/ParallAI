using FakeItEasy;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.SettingsHandlersTests;

[TestFixture]
public abstract class SettingsHandlerTests<THandler, TState>
    where THandler : SettingsHandler<TState>
    where TState : SettingsState
{
    protected ITelegramBotClient Bot;
    protected User User;

    private ChatId chatId;
    private Message message;
    private CallbackQuery query;
    private THandler handler;
    private TState state;

    protected abstract THandler CreateHandler();
    protected abstract TState CreateInitialState();

    [SetUp]
    public void SetUp()
    {
        Bot = A.Fake<ITelegramBotClient>();
        User = new User(1);

        chatId = new ChatId(1);
        message = new Message();
        query = new CallbackQuery { Message = message };
        handler = CreateHandler();
        state = CreateInitialState();
    }

    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    public void ActivatePart_IndexOutOfRangeException(int partIndex)
    {
        Assert.CatchAsync(typeof(IndexOutOfRangeException),
            async () => await handler.ActivatePart(state, partIndex, query, Bot, User));
    }

    [Test]
    public async Task ActivatePart_WhenNextStateIsNotNull()
    {
        await handler.ActivatePart(state, 0, query, Bot, User);
        Assert.That(state.CurrentPart, Is.EqualTo(0));
    }

    [Test]
    public async Task HandleMessage()
    {
        await handler.HandleMessage(state, message, Bot, User);
        Assert.That(state.Reactivated);
    }

    [Test]
    public async Task ExecuteAfter_StateIsNotReactivated()
    {
        state.Reactivated = false;
        var result = await handler.ExecuteAfter(state, chatId, message, Bot);
        Assert.That(result, Is.False);
    }

    [Test]
    public async Task ExecuteAfter_PrevStateIsNotNull()
    {
        state.AcceptPrevState(state.PrevState);

        var result = await handler.ExecuteAfter(state, chatId, message, Bot);
        Assert.That(result, Is.True);
        Assert.That(state.PrevState, Is.Null);
        Assert.That(state.CurrentPart, Is.Null);
        Assert.That(state.Reactivated, Is.False);
    }
}