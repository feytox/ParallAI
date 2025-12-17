using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.Services;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Services;
using ParallAI.TeleBot.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.StateActionsTests;

[TestFixture]
public class RequestStateActionTests
{
    private IGenerationService genService;
    private IMediaGroupCollector mediaGroupCollector;
    private CancelTokenSourceStorage cancelTokenSourceStorage;
    private IMetricService metricService;
    private ITelegramBotClient bot;
    private RequestStateAction action;
    private User user;

    [SetUp]
    public void Setup()
    {
        genService = A.Fake<IGenerationService>();
        mediaGroupCollector = A.Fake<IMediaGroupCollector>();
        cancelTokenSourceStorage = A.Fake<CancelTokenSourceStorage>();
        metricService = A.Fake<IMetricService>();
        bot = A.Fake<ITelegramBotClient>();

        action = new RequestStateAction(genService, mediaGroupCollector, cancelTokenSourceStorage, metricService);
        user = new User(1);
    }

    [Test]
    public async Task Execute_MessagesIsNull_ReturnFalse()
    {
        var msg = new Message();
        var state = A.Fake<RequestState>();

        Message[]? returnValue = null;
        A.CallTo(() => mediaGroupCollector.CollectMessages(A<Message>._)).Returns(returnValue);
        var result = await action.Execute(state, msg, bot, user);

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task Execute_MessagesIsNotNull_ReturnTrue()
    {
        var msg = new Message { Text = "response" };
        var state = A.Fake<RequestState>();

        A.CallTo(() => genService.Generate(
            A<AiModel>._, A<AiMessage[]>._, A<PromptSettings>._, CancellationToken.None))
            .Returns(new AiResponse("response"));
        A.CallTo(() => mediaGroupCollector.CollectMessages(A<Message>._)).Returns([msg]);

        var result = await action.Execute(state, msg, bot, user);
        Assert.That(result, Is.True);
    }
}