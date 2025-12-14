using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.CancelParts;

[CallbackQuery(GeminiProviderSettingsHandler.CancelPartTag)]
public class CancelGeminiProviderPartCallback(IRepository<User, long> users, GeminiProviderSettingsHandler handler): 
    CancelPartCallback<GeminiProviderSettingsState, GeminiProviderSettingsHandler>(users, handler);
    
[CallbackQuery(OpenAICompatibleSettingsHandler.CancelPartTag)]
public class CancelOpenAiProviderPartCallback(IRepository<User, long> users, OpenAICompatibleSettingsHandler handler): 
    CancelPartCallback<OpenAICompatibleSettingsState, OpenAICompatibleSettingsHandler>(users, handler);

[CallbackQuery(OpenRouterSettingsHandler.CancelPartTag)]
public class CancelOpenRouterProviderPartCallback(IRepository<User, long> users, OpenRouterSettingsHandler handler): 
    CancelPartCallback<OpenRouterProviderSettingsState, OpenRouterSettingsHandler>(users, handler);