using FakeItEasy;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;


namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class StateHandlerTests
{
    private IRepository<User, long> userRepo;
    private StateHandler stateHandler;
    private IStateAction action;
    private User user;
    private long userId;
    private ITelegramBotClient bot;
    private UserState state;

    [SetUp]
    public void SetUp()
    {
        userRepo = A.Fake<IRepository<User, long>>();
        action = A.Fake<IStateAction>();
        userId = 1;
        user = new User(userId);
        bot = A.Fake<ITelegramBotClient>();
        A.CallTo(() => userRepo.GetById(userId))!.Returns(Task.FromResult(user));

        var actions = new List<IStateAction> { action };
        stateHandler = new StateHandler(actions, userRepo);
        state = A.Fake<UserState>();
    }

    [Test]
    public async Task HandleState_ShouldActionExecuteAndUserRepoUpdate()
    {
        var msg = new Message { From = new Telegram.Bot.Types.User { Id = userId } };

        user.StateMachine.Push(state);
        A.CallTo(() => action.CanHandle(state)).Returns(true);
        A.CallTo(() => action.Execute(state, msg, bot, user))
            .Returns(ActionResult.Handled);

        await stateHandler.HandleState(msg, bot);
        A.CallTo(() => action.Execute(state, msg, bot, user)).MustHaveHappened();
        A.CallTo(() => userRepo.Update(user)).MustHaveHappened();
    }

    [Test]
    public async Task HandlePostState_ShouldActionExecuteAfterAndUserRepoUpdate()
    {
        var chatId = new ChatId(userId);

        user.StateMachine.Push(state);
        A.CallTo(() => action.CanHandle(state)).Returns(true);
        A.CallTo(() => action.ExecuteAfter(state, chatId, null, bot, user))
            .Returns(ActionResult.Handled);

        await stateHandler.HandlePostState(chatId, userId, bot);
        A.CallTo(() => action.ExecuteAfter(state, chatId, null, bot, user)).MustHaveHappened();
        A.CallTo(() => userRepo.Update(user)).MustHaveHappened();
    }
}