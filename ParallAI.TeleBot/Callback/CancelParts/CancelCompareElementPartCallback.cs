using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback.CancelParts;

[CallbackQuery(CompareElementSettingsHandler.CancelPartTag)]
public class CancelCompareElementPartCallback(IRepository<User, long> users, CompareElementSettingsHandler handler): 
    CancelPartCallback<CompareElementSettingsState, CompareElementSettingsHandler>(users, handler);