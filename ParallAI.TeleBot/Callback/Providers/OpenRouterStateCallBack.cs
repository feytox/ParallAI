using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(OpenRouterSettingsHandler.CallbackTag)]
public class OpenRouterStateCallBack(IRepository<User, long> users, OpenRouterSettingsHandler handler)
    : StandardSettingsStateCallback<OpenRouterProviderSettingsState, OpenRouterSettingsHandler>(users, handler);