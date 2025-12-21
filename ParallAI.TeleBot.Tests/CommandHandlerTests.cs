using FakeItEasy;
using ParallAI.TeleBot.Core.Commands.Common;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class CommandHandlerTests
{
    private ITelegramBotClient bot;
    private User user;

    [SetUp]
    public void Setup()
    {
        bot = A.Fake<ITelegramBotClient>();
        user = new User { Id = 1 };
    }

    [TestCase("/command")]
    [TestCase("          /command       ")]
    [TestCase("/command some text")]
    public async Task HandleCommand_CommandIsExist(string commandName)
    {
        var fakeCmd = A.Fake<ICommand>();
        var commandHandler = CreateHandler((fakeCmd, new CommandAttribute("/command", "test command")));
        var message = new Message
        {
            From = user,
            Text = commandName,
            Chat = new Chat()
        };
        await commandHandler.HandleCommand(message, bot);
        A.CallTo(() => fakeCmd.Execute(message.Chat, message.From.Id, bot)).MustHaveHappened();
    }

    [TestCase("/fakecmd")]
    [TestCase("text /command")]
    public async Task HandleCommand_CommandIsWrong(string commandName)
    {
        var fakeCmd = A.Fake<ICommand>();
        var commandHandler = CreateHandler((fakeCmd, new CommandAttribute("/command", "test command")));
        var message = new Message
        {
            Text = commandName,
            From = user
        };
        await commandHandler.HandleCommand(message, bot);
        A.CallTo(() => fakeCmd.Execute(message.Chat, message.From.Id, bot)).MustNotHaveHappened();
    }

    [Test]
    public void IsHighPriorityCommand_HighPriority()
    {
        var highPriorityCmd = A.Fake<ICommand>();
        var commandHandler = CreateHandler(
            (highPriorityCmd,
                new CommandAttribute("/highprioritycmd", "команда с высоким приоритетом")
                    { HighPriority = true }));
        var highPriorityCmdMessage = new Message { Text = "/highprioritycmd", };

        Assert.That(commandHandler.IsHighPriorityCommand(highPriorityCmdMessage), Is.True);
    }

    [Test]
    public void IsHighPriorityCommand_LowPriority()
    {
        var lowPriorityCmd = A.Fake<ICommand>();

        var commandHandler = CreateHandler(
            (lowPriorityCmd,
                new CommandAttribute("/lowprioritycmd", "команда с низким приоритетом")));
        var lowPriorityCmdMessage = new Message { Text = "/lowprioritycmd", };

        Assert.That(commandHandler.IsHighPriorityCommand(lowPriorityCmdMessage), Is.False);
    }

    private CommandHandler CreateHandler(params (ICommand, CommandAttribute)[] commands)
    {
        return new CommandHandler(commands);
    }
}