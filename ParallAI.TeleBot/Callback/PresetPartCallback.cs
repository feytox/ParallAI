using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(PresetSettingsHandler.Tag)]
public class PresetPartCallback(IRepository<User, long> users, SettingsHandler<PresetSettingsState> handler)
    : SettingsPartCallback<PresetSettingsState>(users, handler);