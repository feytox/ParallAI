using ParallAI.TeleBot.Core.Commands.Common;
using FakeItEasy;
using Telegram.Bot;
using Telegram.Bot.Types;


namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class CommandHandlerTests
{
    private ITelegramBotClient bot;
    
    [SetUp]
    public void Setup()
    {
        bot = A.Fake<ITelegramBotClient>();
    }

    [TestCase("/command")]
    [TestCase("          /command       ")]
    public async Task HandleExistingCommand(string commandName)
    {
        var fakeCmd = A.Fake<ICommand>();
        var commandHandler = CreateHandler((fakeCmd, new CommandAttribute("/command", "test command")));
        var message = new Message
        {
            Text = commandName,
            Chat = new Chat()
        };
        await commandHandler.HandleCommand(message, bot);
        A.CallTo(() => fakeCmd.Execute(message, bot)).MustHaveHappened();
    }
    
    [Test]
    public async Task HandleNonexistentCommand()
    {
        var fakeCmd = A.Fake<ICommand>();
        var commandHandler = CreateHandler();
        var message = new Message
        {
            Text = "/fakecmd",
        };
        await commandHandler.HandleCommand(message, bot);
        A.CallTo(() => fakeCmd.Execute(message, bot)).MustNotHaveHappened();
    }

    [Test]
    public void IsHighPriorityCommand()
    {
        var highPriorityCmd = A.Fake<ICommand>();
        var lowPriorityCmd = A.Fake<ICommand>();
        
        var commandHandler = CreateHandler(
            (highPriorityCmd, new CommandAttribute(
                "/highprioritycmd", "команда с высоким приоритетом") { HighPriority = true }), 
            (lowPriorityCmd, new CommandAttribute(
                "/lowprioritycmd", "команда с низким приоритетом")));

        var highPriorityCmdMessage = new Message { Text = "/highprioritycmd", };
        var lowPriorityCmdMessage = new Message { Text = "/lowprioritycmd", };

        Assert.That(commandHandler.IsHighPriorityCommand(highPriorityCmdMessage));
        Assert.That(!commandHandler.IsHighPriorityCommand(lowPriorityCmdMessage));
    }
    
    private CommandHandler CreateHandler(params (ICommand, CommandAttribute)[] commands)
    {
        return new CommandHandler(commands);
    }
}