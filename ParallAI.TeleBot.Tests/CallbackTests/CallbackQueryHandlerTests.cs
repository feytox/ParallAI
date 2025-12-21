using FakeItEasy;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests.CallbackTests;

[TestFixture]
public class CallbackQueryHandlerTests
{
    private ITelegramBotClient bot;
    private ICallbackQuery callback;
    private string key;

    [SetUp]
    public void Setup()
    {
        bot = A.Fake<ITelegramBotClient>();
        callback = A.Fake<ICallbackQuery>();
        key = "test-key";
    }

    [Test]
    public async Task HandleCallbackQuery_WhenExists()
    {
        var callbackQueryHandler = CreateHandler((callback, new CallbackQueryAttribute(key)));
        var callbackQuery = new CallbackQuery { Data = key };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => callback.Handle(
            callbackQuery, A<CallbackData>.That.Matches(d => d.Key == key), bot)).MustHaveHappened();
    }

    [Test]
    public async Task HandleCallbackQuery_WhenNotExists()
    {
        var callbackQueryHandler = CreateHandler();
        var callbackQuery = new CallbackQuery { Data = key };
        await callbackQueryHandler.HandleCallbackQuery(callbackQuery, bot);
        A.CallTo(() => callback.Handle(
            callbackQuery, A<CallbackData>.That.Matches(d => d.Key == key), bot)).MustNotHaveHappened();
    }

    private CallbackQueryHandler CreateHandler(params (ICallbackQuery, CallbackQueryAttribute)[] callbackQueries)
    {
        return new CallbackQueryHandler(callbackQueries);
    }
}