using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(GeminiProviderSettingsHandler.Tag)]
public class GeminiPartCallBack(IRepository<User, long> users, SettingsHandler<GeminiProviderSettingsState> handler)
    : SettingsPartCallback<GeminiProviderSettingsState>(users, handler);