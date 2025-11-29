using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Util;

public static class KeyboardHelper
{
    public static ReplyKeyboardMarkup CreateReplyKeyboard(IEnumerable<string> buttonNames, int columns = 2, string? placeholder = null)
    {
        var buttons = new List<KeyboardButton[]>();
        var currentRow = new List<KeyboardButton>();

        foreach (var name in buttonNames)
        {
            currentRow.Add(new KeyboardButton(name));

            if (currentRow.Count != columns) continue;
            
            buttons.Add(currentRow.ToArray());
            currentRow = [];
        }
        
        if (currentRow.Count > 0)
            buttons.Add(currentRow.ToArray());

        return new ReplyKeyboardMarkup(buttons)
        {
            ResizeKeyboard = true,
            InputFieldPlaceholder = placeholder
        };
    }
}