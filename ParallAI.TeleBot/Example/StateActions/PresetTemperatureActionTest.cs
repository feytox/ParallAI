// using ParallAI.TeleBot.Example.States;
// using ParallAI.TeleBot.StateActions.Common;
// using Telegram.Bot;
// using Telegram.Bot.Types;
// using User = ParallAI.Core.Entities.User;
//
// namespace ParallAI.TeleBot.Example.StateActions;
//
// public class PresetTemperatureActionTest : IStepStateAction<PresetState, PresetStep>
// {
//     public PresetStep StateStep => PresetStep.Temperature;
//     
//     public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
//     {
//         if (!float.TryParse(message.Text?.Replace(',', '.'), out var temperature) || temperature < 0 || temperature > 2)
//         {
//             await bot.SendMessage(message.Chat,
//                 "Это не похоже на правильное число. Температура должна быть от 0 до 2. Попробуй ещё раз.");
//             return false;
//         }
//         
//         state.Temperature = temperature;
//
//         await bot.SendMessage(message.Chat, $"Бака-братик, теперь у нас есть новый пресет!");
//         return true;
//     }
// }