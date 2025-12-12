using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using FakeItEasy;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests;

[TestFixture]
public abstract class SettingsHandlerTests<THandler, TState>
    where THandler : SettingsHandler<TState>
    where TState : SettingsState
{
    protected ITelegramBotClient Bot;
    protected User User;
    protected ChatId ChatId;
    
    protected abstract THandler CreateHandler();
    protected abstract TState CreateInitialState();

    [SetUp]
    public void SetUp()
    {
        Bot = A.Fake<ITelegramBotClient>();
        User = new User(1);
        ChatId = new ChatId(1);
    }

    [TestCase(-1)]
    [TestCase(int.MaxValue)]
    public void ActivatePart_IndexOutOfRangeException(int partIndex)
    {
        var state = CreateInitialState();
        var handler = CreateHandler();
        Assert.CatchAsync(typeof(IndexOutOfRangeException),
            async () => await handler.ActivatePart(state, partIndex, ChatId, Bot, User));
    }

    [Test] 
    public async Task ActivatePart_WhenNextStateIsNotNull()
    {
        var state = CreateInitialState();
        var handler = CreateHandler();
        await handler.ActivatePart(state, 0, ChatId, Bot, User);
        Assert.That(state.CurrentPart, Is.EqualTo(0));
    }
    
    [Test]
    public async Task HandleMessage()
    {
        var state = CreateInitialState();
        var msg = new Message();
        var handler = CreateHandler();
        
        await handler.HandleMessage(state, msg, Bot, User);
        
        Assert.That(state.Reactivated);
    }

    [Test]
    public async Task ExecuteAfter_StateIsNotReactivated()
    {
        var state = CreateInitialState();
        var handler = CreateHandler();
        state.Reactivated = false;
        
        var result = await handler.ExecuteAfter(state, ChatId, Bot);
        Assert.That(result, Is.False);
    }
    
    [Test]
    public async Task ExecuteAfter_PrevStateIsNotNull()
    {
        var state = CreateInitialState();
        var handler = CreateHandler();
        state.AcceptPrevState(state.PrevState);
        
        var result = await handler.ExecuteAfter(state, ChatId, Bot);
        Assert.That(result, Is.True);
        Assert.That(state.PrevState, Is.Null);
        Assert.That(state.CurrentPart, Is.Null);
        Assert.That(state.Reactivated, Is.False);
    }
}