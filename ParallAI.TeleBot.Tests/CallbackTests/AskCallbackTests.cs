using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using FluentAssertions;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.CallbackTests;

[TestFixture]
public class AskCallbackTests
{
    private IRepository<User, long> userRepo;
    private TestableAskCallback askCallback;
    private User user;
    private ITelegramBotClient bot;
    private string key;
    private CallbackQuery query;

    [SetUp]
    public void Setup()
    {
        userRepo = A.Fake<IRepository<User, long>>();
        askCallback = new TestableAskCallback(userRepo);
        user = new User(1);
        bot = A.Fake<ITelegramBotClient>();
        key = "ask";
        query = new CallbackQuery { Message = new Message { Chat = new Chat(), Id = 1} };
    }


    [TestCase(RequestMode.Single, "single")]
    [TestCase(RequestMode.Continuous, "continuous")]
    public async Task Handle_ChooseRequestConfig(RequestMode mode, string stringMode)
    {
        var model = A.Fake<AiModel>();
        var preset = A.Fake<Preset>();
        user.AddModel(model);
        user.ChooseModel(model);
        user.AddPreset(preset);
        user.ChoosePreset(preset);
        var data = new CallbackData($"{key}:{stringMode}");
        var argsData = new CallbackData<AskArgs>(data);

        var expectedConfig = new RequestConfig(model, preset, mode);
        var expectedCurrentState = new RequestState(expectedConfig);

        await askCallback.CallHandle(query, argsData, bot, user);


        Assert.That(user.StateMachine.Current, Is.TypeOf(typeof(RequestState)));
        user.StateMachine.Current.Should().BeEquivalentTo(expectedCurrentState);
    }

    [Test]
    public async Task Handle_CreateRequestConfig()
    {
        var stringMode = "settings";
        var data = new CallbackData($"{key}:{stringMode}");
        var argsData = new CallbackData<AskArgs>(data);

        await askCallback.CallHandle(query, argsData, bot, user);

        var expectedState = new RequestSettingsState();
        user.StateMachine.Current.Should().BeEquivalentTo(expectedState);
    }

    [TestCase(RequestMode.Single, "single")]
    [TestCase(RequestMode.Continuous, "continuous")]
    public async Task Handle_ChooseRequestConfigWithNullModel_ShouldEditMessage(RequestMode mode, string stringMode)
    {
        A.CallTo(() => bot.SendRequest(A<IRequest<Message>>._, A<CancellationToken>._))
            .Returns(Task.FromResult(new Message()));
        var data = new CallbackData($"{key}:{stringMode}");
        var argsData = new CallbackData<AskArgs>(data);

        await askCallback.CallHandle(query, argsData, bot, user);

        A.CallTo(() => bot.SendRequest(A<EditMessageTextRequest>._, A<CancellationToken>._))
            .MustHaveHappened();
        Assert.That(user.StateMachine.Current, Is.TypeOf(typeof(MainMenuState)));
    }
}

class TestableAskCallback(IRepository<User, long> users) : AskCallback(users)
{
    public async Task CallHandle(CallbackQuery query, CallbackData<AskArgs> data, ITelegramBotClient bot, User user)
    {
        await base.Handle(query, data, bot, user);
    }
}