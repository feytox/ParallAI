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
    protected ChatId ChatId;
    
    private Message message;
    private CallbackQuery query;
    private THandler handler;
    
    protected abstract THandler CreateHandler();
    protected abstract TState CreateInitialState();

    [SetUp]
    public void SetUp()
    {
        Bot = A.Fake<ITelegramBotClient>();
        User = new User(1);
        ChatId = new ChatId(1);
        
        message = new Message();
        query = new CallbackQuery { Message = message };
        handler = CreateHandler();
    }

    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    public void ActivatePart_IndexOutOfRangeException(int partIndex)
    {
        var state = CreateInitialState();
        Assert.CatchAsync(typeof(IndexOutOfRangeException),
            async () => await handler.ActivatePart(state, partIndex, query, Bot, User));
    }

    [Test] 
    public async Task ActivatePart_WhenNextStateIsNotNull()
    {
        var state = CreateInitialState();
        await handler.ActivatePart(state, 0, query, Bot, User);
        Assert.That(state.CurrentPart, Is.EqualTo(0));
    }
    
    [Test]
    public async Task HandleMessage()
    {
        var state = CreateInitialState();
        await handler.HandleMessage(state, message, Bot, User);
        Assert.That(state.Reactivated);
    }

    [Test]
    public async Task ExecuteAfter_StateIsNotReactivated()
    {
        var state = CreateInitialState();
        state.Reactivated = false;
        
        var result = await handler.ExecuteAfter(state, ChatId, message, Bot);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task ExecuteAfter_PrevStateIsNotNull()
    {
        var state = CreateInitialState();
        state.AcceptPrevState(state.PrevState);
        
        var result = await handler.ExecuteAfter(state, ChatId, message, Bot);
        Assert.That(result, Is.True);
        Assert.That(state.PrevState, Is.Null);
        Assert.That(state.CurrentPart, Is.Null);
        Assert.That(state.Reactivated, Is.False);
    }
}