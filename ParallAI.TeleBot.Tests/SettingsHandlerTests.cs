using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Settings;
using FakeItEasy;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Tests;

[TestFixture]
public abstract class SettingsHandlerTests<THandler, TState>
    where THandler : SettingsHandler<TState>
    where TState : SettingsState
{
    protected ITelegramBotClient Bot;
    protected User User;
    protected ChatId ChatId;

    [SetUp]
    public void SetUp()
    {
        Bot = A.Fake<ITelegramBotClient>();
        User = new User(1);
        ChatId = new ChatId(1);
    }

    protected abstract THandler CreateHandler();
}