using AICore;
using Infrastructure;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.User;

namespace TeleBot.Commands;

// временная команда, в будущем пресеты должны добавляться в поэтапном режиме
[Command("/addpreset", "добавление пользовательского пресета")]
public class AddPresetCommand(IRepository<User, long> users, ILogger<AddPresetCommand> logger) : ICommand
{
    public async Task Execute(Message message, ITelegramBotClient bot)
    {
        var userId = message.From!.Id;
        var user = users.GetOrThrow(userId);
        
        var presetId = BitConverter.ToInt64(Guid.NewGuid().ToByteArray(), 0);
        var messageSplit = message.Text!.Split(' ');
        if (messageSplit.Length < 2)
        {
            await bot.SendMessage(message.Chat,"Вы не ввели имя пресета");
            return;
        }
        
        var presetName = messageSplit[1];
        user.Result.Presets.Add(new Preset(presetId, presetName));
        await bot.SendMessage(message.Chat, $"Пресет {presetName} добавлен");
        logger.LogInformation($"Пресет {presetId} добавлен к пользователю {userId}");
    }
}