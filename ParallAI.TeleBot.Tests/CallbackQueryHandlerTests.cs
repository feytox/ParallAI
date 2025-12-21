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
    public async Task HandleCallbackQuery_WhenExists()
    {
        var fakeClb = A.Fake<ICallbackQuery>();
        var key = "test-key";
        var callbackQueryHandler = CreateHandler((fakeClb, new CallbackQueryAttribute(key)));
        var callbackQuery = new CallbackQuery { Data = key };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => fakeClb.Handle(
            callbackQuery, A<CallbackData>.That.Matches(d => d.Key == key), bot)).MustHaveHappened();
    }

    [Test]
    public async Task HandleCallbackQuery_WhenNotExists()
    {
        var fakeClb = A.Fake<ICallbackQuery>();
        var key = "test-key";
        var callbackQueryHandler = CreateHandler();
        var callbackQuery = new CallbackQuery { Data = key };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => fakeClb.Handle(
            callbackQuery, A<CallbackData>.That.Matches(d => d.Key == key), bot)).MustNotHaveHappened();
    }

    private CallbackQueryHandler CreateHandler(params (ICallbackQuery, CallbackQueryAttribute)[] callbackQueries)
    {
        return new CallbackQueryHandler(callbackQueries);
    }
}