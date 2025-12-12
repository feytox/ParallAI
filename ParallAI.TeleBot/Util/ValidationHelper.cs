using Telegram.Bot;
using Telegram.Bot.Types;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Util;

public static class ValidationHelper
{
    public static async Task<bool> ValidateModelsCount(ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (user.UserModels.Count > 0)
            return true;
        
        await bot.SendMessage(chatId, "У вас 0 настроенных моделей.\nИспользуйте /models для настройки");
        return false;
    }
    
    public static async Task<bool> ValidatePresetsCount(ChatId chatId, ITelegramBotClient bot, User user)
    {
        if (user.UserPresets.Count > 0)
            return true;
        
        await bot.SendMessage(chatId, "У вас 0 настроенных пресетов.\nИспользуйте /presets для настройки");
        return false;
    }
}