using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Settings;

public class CompareSettingsHandler() : SettingsHandler<CompareSettingsState>(CallbackTag, CreateParts)
{
    public const string CallbackTag = "compare-settings";
    private const int MinElements = 2;
    
    public void RemoveCompareElement(CompareSettingsState state, int index)
    {
        state.RemoveAt(index);
        state.Reactivated = true;
    }

    protected override async Task SendPartsList(CompareSettingsState state, ChatId chatId, ITelegramBotClient bot)
    {
        var buttons = state.ConfiguredElements
            .Select(e => $"Модель: {e.Model.DisplayName}    Пресет: {e.Preset.Name}")
            .Select((elementText, i) => InlineKeyboardButton.WithCallbackData(elementText, $"{CallbackTag}:{-i - 1}"))
            .Concat(CreatePartButtons(state))
            .Chunk(1)
            .ToList();
        
        if (state.ConfiguredElements.Count >= MinElements)
            buttons.Add([InlineKeyboardButton.WithCallbackData("Начать сравнение", $"{CallbackTag}:s")]);

        buttons.Add([CancelCallback.CreateButton("Выйти без сохранения")]);

        // TODO: rewrite text
        await bot.SendMessage(chatId, 
            $"Для старта сравнения настройте как минимум {MinElements} элемента.\n\n" +
            "Для удаления элемента нажмите на него",
            replyMarkup: new InlineKeyboardMarkup(buttons));
    }

    protected override async Task<bool> SaveSettingsToUser(CompareSettingsState state, ChatId chatId, 
        ITelegramBotClient bot, User user)
    {
        user.StateMachine.Pop(reactivate: false);
        var nextState = state.ToOrchestratorState();
        user.StateMachine.Push(nextState);

        await bot.SendMessage(chatId, "Теперь настройте оркестратора.");
        return false;
    }

    private static void CreateParts(SettingsPartsBuilder<CompareSettingsState> builder)
    {
        builder.Add(new CreateElementSettingsPart("+"));
    }
}