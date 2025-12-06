using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(ModelSettingsHandler.CallbackTag)]
public class ModelStateCallBack(IRepository<User, long> users, ModelSettingsHandler handler)
    : StandardSettingsStateCallback<ModelSettingsState, ModelSettingsHandler>(users, handler);