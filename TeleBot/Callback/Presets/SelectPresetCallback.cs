using AICore.Entities;
using AICore.Repositories;
using TeleBotInfr.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = AICore.Entities.User;

namespace TeleBot.Callback.Presets;

// временно
[CallbackQuery("selectpreset")]
public class SelectPresetCallback(IRepository<User, long> users) : PresetCallbackQuery(users)
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user, Preset preset)
    {
        await bot.SendMessage(callbackQuery.From.Id, "Пресет выбран");
        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}