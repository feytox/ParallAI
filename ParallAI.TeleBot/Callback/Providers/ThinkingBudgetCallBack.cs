using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(PresetSettingsHandler.ThinkingBudgetTag)]
public class ThinkingBudgetCallBack(IRepository<User, long> users, SettingsHandler<PresetSettingsState> handler) : 
    SettingsStateCallBack<PresetSettingsState>(users, handler);