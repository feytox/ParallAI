using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback.CancelParts;

[CallbackQuery(PresetSettingsHandler.CancelPartTag)]
public class CancelPresetPartCallback(IRepository<User, long> users, PresetSettingsHandler handler): 
    CancelPartCallback<PresetSettingsState, PresetSettingsHandler>(users, handler);