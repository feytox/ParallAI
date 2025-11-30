using ParallAI.TeleBot.Commands.UI;
using ParallAI.TeleBot.Core.Commands;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ParallAI.TeleBot.Commands;

[MainMenu("🛠️ Настройка конфигурации", MenuOrder.Config)]
//[Command("/config", "настройка конфигурации")]
public class ConfigurationCommand : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        await bot.SendMessage(message.Chat, "Сделай меня :3");
    }
}