using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(GeminiProviderSettingsHandler.CallbackTag)]
public class GeminiStateCallBack(IRepository<User, long> users, GeminiProviderSettingsHandler handler)
    : StandardSettingsStateCallback<GeminiProviderSettingsState, GeminiProviderSettingsHandler>(users, handler);