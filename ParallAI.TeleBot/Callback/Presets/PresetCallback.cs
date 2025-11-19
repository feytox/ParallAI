using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.TeleBot.Core.Callback;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.Presets;

[CallbackQuery("preset")]
public class PresetCallback(IRepository<User, long> users) : PresetCallbackQuery(users)
{
    protected override async Task Handle(CallbackQuery callbackQuery, ITelegramBotClient bot, User user, Preset preset)
    {
        // TODO: нужно будет отрефакторить, когда будем добавлять другие параметры
        var message = $"Пресет \"{preset.Name}\"\n" +
                      $"Системный промпт: {preset.PromptSettings.SystemPrompt}\n" +
                      $"Температура: {preset.PromptSettings.Temperature}\n" +
                      $"Бюджет размышлений: {preset.PromptSettings.ThinkingBudget}";
        var presetActions = new[]
        {
            InlineKeyboardButton.WithCallbackData("Выбрать", $"selectpreset:{preset.Id}"),
            InlineKeyboardButton.WithCallbackData("Изменить", $"changepreset:{preset.Id}"),
            InlineKeyboardButton.WithCallbackData("Удалить", $"deletepreset:{preset.Id}")
        };
        await bot.SendMessage(callbackQuery.From.Id, message, replyMarkup: new InlineKeyboardMarkup(presetActions));
        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }
}