using FakeItEasy;
using ParallAI.Core.States;
using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.StateActions;
using ParallAI.TeleBot.StateActions;
using Telegram.Bot;
using Telegram.Bot.Types;
using ICommand = ParallAI.TeleBot.Core.Commands.Common.ICommand;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests.StateActionsTests;

[TestFixture]
public class MainMenuStateActionTests
{
    private ITelegramBotClient bot;
    private User user;
    private ChatId chatId;

    [SetUp]
    public void Setup()
    {
        bot = A.Fake<ITelegramBotClient>();
        user = new User(1);
        chatId = new ChatId(1);
    }

    [Test]
    public async Task Execute_CommandExecute_ActionHandled()
    {
        var state = new MainMenuState();
        var msg = new Message {Text = "command", Chat = new Chat(), From = new Telegram.Bot.Types.User()};

        var command = A.Fake<ICommand>();
        A.CallTo(() => command.Execute(A<ChatId>._, A<long>._, A<ITelegramBotClient>._)).Returns(Task.CompletedTask);

        var attr = new MainMenuAttribute("command", MenuOrder.Ask);
        var commandStorage = CreateMainMenuCommandsStorage((command, attr));

        var action = new MainMenuStateAction(commandStorage);
        var result = await action.Execute(state, msg, bot, user);

        Assert.That(result, Is.EqualTo(ActionResult.Handled));
        A.CallTo(() => command.Execute(A<ChatId>._, A<long>._, A<ITelegramBotClient>._)).MustHaveHappened();
    }

    [Test]
    public async Task Execute_CommandNotFound_ActionSkipped()
    {
        var state = new MainMenuState();
        var msg = new Message {Text = "command", Chat = new Chat(), From = new Telegram.Bot.Types.User()};

        var command = A.Fake<ICommand>();
        var attr = new MainMenuAttribute("another command", MenuOrder.Ask);
        var commandStorage = CreateMainMenuCommandsStorage((command, attr));

        var action = new MainMenuStateAction(commandStorage);
        var result = await action.Execute(state, msg, bot, user);

        Assert.That(result, Is.EqualTo(ActionResult.Skipped));
    }

    [Test]
    public async Task Execute_TextIsNull_ActionSkipped()
    {
        var state = new MainMenuState();
        var msg = new Message {Chat = new Chat(), From = new Telegram.Bot.Types.User()};
        var commandStorage = CreateMainMenuCommandsStorage();

        var action = new MainMenuStateAction(commandStorage);
        var result = await action.Execute(state, msg, bot, user);

        Assert.That(result, Is.EqualTo(ActionResult.Skipped));
    }

    [Test]
    public async Task ExecuteAfter_StateReactivated_ActionHandled()
    {
        var state = new MainMenuState { Reactivated = true };
        var commandStorage = CreateMainMenuCommandsStorage();
        var action = new MainMenuStateAction(commandStorage);
        var result = await action.ExecuteAfter(state, chatId, null, bot, user);
        Assert.That(result, Is.EqualTo(ActionResult.Handled));
        Assert.That(state.Reactivated, Is.False);
    }
    
    [Test]
    public async Task ExecuteAfter_StateNotReactivated_ActionSkipped()
    {
        var state = new MainMenuState { Reactivated = false };
        var commandStorage = CreateMainMenuCommandsStorage();
        var action = new MainMenuStateAction(commandStorage);
        var result = await action.ExecuteAfter(state, chatId, null, bot, user);
        Assert.That(result, Is.EqualTo(ActionResult.Skipped));
    }

    private MainMenuCommandsStorage CreateMainMenuCommandsStorage(params IEnumerable<(ICommand command, MainMenuAttribute attribute)> commands)
    {
        return new MainMenuCommandsStorage(commands);
    }
}