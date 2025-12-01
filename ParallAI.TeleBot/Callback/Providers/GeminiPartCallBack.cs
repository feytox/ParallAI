using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(GeminiProviderSettingsHandler.Tag)]
public class GeminiPartCallBack(IRepository<User, long> users, SettingsHandler<GeminiProviderSettingsState> handler)
    : SettingsPartCallback<GeminiProviderSettingsState>(users, handler);