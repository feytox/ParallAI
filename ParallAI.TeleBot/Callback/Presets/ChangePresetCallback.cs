using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.Presets;

// TODO: нужно будет переделать
[CallbackQuery("changepreset")]
public class ChangePresetCallback(IRepository<User, long> users) : PresetCallbackQuery(users)
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user, Preset preset)
    {
        // TODO: нужно будет отрефакторить, когда будем добавлять другие параметры
        var presetChanges = new[]
        {
            InlineKeyboardButton.WithCallbackData(
                "Название", $"changepresetname:{preset.Id}"),
            InlineKeyboardButton.WithCallbackData(
                "Системный промпт", $"changepresetsystemprompt:{preset.Id}"),
            InlineKeyboardButton.WithCallbackData(
                "Температура", $"changepresettemperature:{preset.Id}"),
            InlineKeyboardButton.WithCallbackData(
                "Бюджет размышлений", $"changepresetthinkingbudget:{preset.Id}")
        }.Chunk(1);
        
        await bot.SendMessage(
            callbackQuery.From.Id, "Выберите параметр для изменения:",
            replyMarkup: new InlineKeyboardMarkup(presetChanges));
        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}