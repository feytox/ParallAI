// using ParallAI.TeleBot.Example.States;
// using ParallAI.TeleBot.StateActions.Common;
// using Telegram.Bot;
// using Telegram.Bot.Types;
// using User = ParallAI.Core.Entities.User;
//
// namespace ParallAI.TeleBot.Example.StateActions;
//
// public class PresetPromptActionTest : IStepStateAction<PresetStateTest, PresetStep>
// {
//     public PresetStep StateStep => PresetStep.SystemPrompt;
//     
//     public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
//     {
//         if (string.IsNullOrWhiteSpace(message.Text))
//         {
//             await bot.SendMessage(message.Chat, "Промпт не может быть пустым.");
//             return false;
//         }
//         
//         state.SystemPrompt = message.Text;
//         
//         await bot.SendMessage(message.Chat, "ПЕРВЫЙ СКИЛ И ТРЕТИЙ БЛЯЯЯТЬ! Укажи температуру.");
//         return true;
//     }
// }