using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.Presets;

[CallbackQuery("deletepreset")]
public class DeletePresetCallback(IRepository<User, long> users) : PresetCallbackQuery(users)
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user, Preset preset)
    {
        user.DeletePreset(preset);
        await bot.SendMessage(callbackQuery.From.Id, $"Пресет {preset.Name} был удалён");
        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}