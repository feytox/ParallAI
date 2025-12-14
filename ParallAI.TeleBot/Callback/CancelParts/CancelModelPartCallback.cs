using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;
using User = ParallAI.Core.Entities.User;

namespace ParallAI.TeleBot.Callback.CancelParts;

[CallbackQuery(ModelSettingsHandler.CancelPartTag)]
public class CancelModelPartCallback(IRepository<User, long> users, ModelSettingsHandler handler): 
    CancelPartCallback<ModelSettingsState, ModelSettingsHandler>(users, handler);