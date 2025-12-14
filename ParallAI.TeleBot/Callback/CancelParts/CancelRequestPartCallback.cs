using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback.CancelParts;

[CallbackQuery(RequestSettingsHandler.CancelPartTag)]
public class CancelRequestPartCallback(IRepository<User, long> users, RequestSettingsHandler handler): 
    CancelPartCallback<RequestSettingsState, RequestSettingsHandler>(users, handler);
