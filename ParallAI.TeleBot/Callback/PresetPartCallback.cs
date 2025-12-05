using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(PresetSettingsHandler.CallbackTag)]
public class PresetPartCallback(IRepository<User, long> users, PresetSettingsHandler handler)
    : StandardSettingsPartCallback<PresetSettingsState, PresetSettingsHandler>(users, handler);