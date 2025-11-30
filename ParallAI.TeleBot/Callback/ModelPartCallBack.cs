using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Common;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(ModelSettingsHandler.Tag)]
public class ModelPartCallBack(IRepository<User, long> users, SettingsHandler<ModelSettingsState> handler)
    : SettingsPartCallback<ModelSettingsState>(users, handler);