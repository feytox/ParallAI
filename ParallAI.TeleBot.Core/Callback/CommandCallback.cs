using System.Reflection;
using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Commands.Common;
using ParallAI.TeleBot.Core.Util;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace ParallAI.TeleBot.Core.Callback;

[CallbackQuery(Tag)]
public class CommandCallback(CommandHandler commandHandler) : TypedCallbackQuery<CommandArgs>
{
    private const string Tag = "command";

    protected override async Task Handle(CallbackQuery query, CallbackData<CommandArgs> data, ITelegramBotClient bot)
    {
        await bot.DeleteCallbackMessage(query);

        var command = data.Args.CommandName;
        await commandHandler.HandleCommand(command, query.GetChatId(), query.GetChatId().Identifier!.Value, bot);
    }

    public static InlineKeyboardButton Create<T>(string text) where T : ICommand
    {
        var type = typeof(T);
        var attribute = type.GetCustomAttribute<CommandAttribute>() 
                        ?? throw new ArgumentException($"{type} doesn't have CommandAttribute");
        return Create(text, attribute.Name);
    }

    public static InlineKeyboardButton Create(string text, string command)
    {
        return InlineKeyboardButton.WithCallbackData(text, $"{Tag}:{command}");
    }
}