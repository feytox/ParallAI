using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.OldPresets;

public abstract class PresetCallbackQuery(IRepository<User, long> users) : UserCallbackQuery(users)
{
    protected abstract Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user, Preset preset);

    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user)
    {
        var presetId = callbackQuery.Data!.Split(':')[1];
        var preset = user.UserPresets.FirstOrDefault(p => p.Id == Guid.Parse(presetId));
        if (preset is null)
        {
            await bot.SendMessage(callbackQuery.From.Id, "Пресет не найден");
            await bot.AnswerCallbackQuery(callbackQuery.Id);
            return;
        }
        await Handle(callbackQuery, bot, user, preset);
        await Users.Update(user);
    }
}