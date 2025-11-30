using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🤖 Модели", MenuOrder.Models)]
//[Command("/models", "список моделей")]
public class ModelsCommand : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Сделай меня :3");
    }
}