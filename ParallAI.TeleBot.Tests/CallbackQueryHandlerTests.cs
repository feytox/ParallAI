using FakeItEasy;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class CallbackQueryHandlerTests
{
    private ITelegramBotClient bot;
    
    [SetUp]
    public void Setup()
    {
        bot = A.Fake<ITelegramBotClient>();
    }

    [Test]
    public async Task HandleExistingCallbackQuery()
    {
        var fakeClb = A.Fake<ICallbackQuery>();
        var callbackQueryHandler = CreateHandler((fakeClb, new CallbackQueryAttribute("test-key")));
        var callbackQuery = new CallbackQuery { Data = "test-key" };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => fakeClb.Handle(callbackQuery, bot)).MustHaveHappened();
    }
    
    [Test]
    public async Task HandleNonexistentCallbackQuery()
    {
        var fakeClb = A.Fake<ICallbackQuery>();
        var callbackQueryHandler = CreateHandler();
        var callbackQuery = new CallbackQuery { Data = "test-key" };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => fakeClb.Handle(callbackQuery, bot)).MustNotHaveHappened();
    }

    private CallbackQueryHandler CreateHandler(params (ICallbackQuery, CallbackQueryAttribute)[] callbackQueries)
    {
        return new CallbackQueryHandler(callbackQueries);
    }
}