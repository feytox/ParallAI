using FakeItEasy;
using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.StateActionsTests;

[TestFixture]
public class CompareStateActionTests
{
    private ComparisonService comparisonService;
    private IMediaGroupCollector mediaGroupCollector;
    private CancelTokenSourceStorage cancelTokenSourceStorage;
    private ITelegramBotClient bot;
    private CompareStateAction action;
    private User user;

    [SetUp]
    public void Setup()
    {
        comparisonService = A.Fake<ComparisonService>();
        mediaGroupCollector = A.Fake<IMediaGroupCollector>();
        cancelTokenSourceStorage = A.Fake<CancelTokenSourceStorage>();
        bot = A.Fake<ITelegramBotClient>();

        action = new CompareStateAction(comparisonService, mediaGroupCollector, cancelTokenSourceStorage);
        user = new User(1);
    }

    [Test]
    public async Task Execute_MessagesIsNull_ActionHandledCompletely()
    {
        var msg = new Message();
        var state = A.Fake<CompareState>();

        Message[]? returnValue = null;
        A.CallTo(() => mediaGroupCollector.CollectMessages(A<Message>._)).Returns(returnValue);
        var result = await action.Execute(state, msg, bot, user);

        Assert.That(result, Is.EqualTo(ActionResult.HandledCompletely));
    }

    [Test]
    public async Task Execute_MessagesIsNotNull_ActionHandled()
    {
        var msg = new Message { Text = "text" };
        var state = A.Fake<CompareState>();

        A.CallTo(() => mediaGroupCollector.CollectMessages(A<Message>._)).Returns([msg]);
        var result = await action.Execute(state, msg, bot, user);
        Assert.That(result, Is.EqualTo(ActionResult.Handled));
    }
}