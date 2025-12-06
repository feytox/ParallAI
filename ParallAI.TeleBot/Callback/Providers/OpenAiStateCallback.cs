using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(OpenAICompatibleSettingsHandler.CallbackTag)]
public class OpenAiStateCallback(IRepository<User, long> users, OpenAICompatibleSettingsHandler handler)
    : StandardSettingsStateCallback<OpenAICompatibleSettingsState, OpenAICompatibleSettingsHandler>(users, handler);