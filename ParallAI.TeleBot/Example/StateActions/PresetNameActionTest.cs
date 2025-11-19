// using ParallAI.TeleBot.Example.States;
// using ParallAI.TeleBot.StateActions.Common;
// using Telegram.Bot;
// using Telegram.Bot.Types;
// using User = ParallAI.Core.Entities.User;
//
// namespace ParallAI.TeleBot.Example.StateActions;
//
// public class PresetNameActionTest : IStepStateAction<PresetState, PresetStep>
// {
//     public PresetStep StateStep => PresetStep.Name;
//     
//     public async Task<bool> Execute(PresetState state, Message message, ITelegramBotClient bot, User user)
//     {
//         state.Name = message.Text;
//         await bot.SendMessage(message.Chat, "Ямете кудасай! Теперь выбери модель.");
//         return true;
//     }
// }