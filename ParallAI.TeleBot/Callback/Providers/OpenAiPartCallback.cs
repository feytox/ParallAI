using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(OpenAICompatibleSettingsHandler.CallbackTag)]
public class OpenAiPartCallback(IRepository<User, long> users, OpenAICompatibleSettingsHandler handler)
    : StandardSettingsPartCallback<OpenAICompatibleSettingsState, OpenAICompatibleSettingsHandler>(users, handler);