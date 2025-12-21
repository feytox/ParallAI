using FakeItEasy;
using FluentAssertions;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Callback;
using ParallAI.TeleBot.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Callback.Common;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.CallbackTests;

[TestFixture]
public class CompareCallbackTests
{
    private IRepository<User, long> userRepo;
    private TestableCompareCallback compareCallback;
    private User user;
    private ITelegramBotClient bot;
    private string key;
    private CallbackQuery query;

    [SetUp]
    public void Setup()
    {
        userRepo = A.Fake<IRepository<User, long>>();
        compareCallback = new TestableCompareCallback(userRepo);
        user = new User(1);
        bot = A.Fake<ITelegramBotClient>();
        key = "compare";
        query = new CallbackQuery { Message = new Message { Chat = new Chat(), Id = 1} };
    }

    [Test]
    public async Task Handle_CreateCompareConfig_CurrentStateIsCompareSettings()
    {
        var stringAction = "+";
        var data = new CallbackData($"{key}:{stringAction}");
        var argsData = new CallbackData<CompareArgs>(data);

        await compareCallback.CallHandle(query, argsData, bot, user);

        var expectedState = new CompareSettingsState();
        user.StateMachine.Current.Should().BeEquivalentTo(expectedState);
    }

    [Test]
    public async Task ChooseCompareConfig_CurrentStateIsCompareState()
    {
        var compareConfig = A.Fake<CompareConfig>();
        user.AddComparison(compareConfig, 100);

        var index = 0;
        var data = new CallbackData($"{key}:{index}");
        var argsData = new CallbackData<CompareArgs>(data);
        await compareCallback.CallHandle(query, argsData, bot, user);

        var expectedState = new CompareState(user.Comparisons[index]);
        user.StateMachine.Current.Should().BeEquivalentTo(expectedState);
    }
}

class TestableCompareCallback(IRepository<User, long> users) : CompareCallback(users)
{
    public Task CallHandle(CallbackQuery query, CallbackData<CompareArgs> data, ITelegramBotClient bot, User user)
    {
        return base.Handle(query, data, bot, user);
    }
}