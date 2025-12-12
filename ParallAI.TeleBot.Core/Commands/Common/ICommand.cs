using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Core.Commands.Common;

public interface ICommand
{
    Task Execute(ChatId chatId, long userId, ITelegramBotClient bot);
}