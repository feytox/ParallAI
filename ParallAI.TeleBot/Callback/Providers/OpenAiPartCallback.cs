using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States.Providers;
using ParallAI.TeleBot.Core.Callback;
using ParallAI.TeleBot.Core.Settings;
using ParallAI.TeleBot.Settings.Provider;

namespace ParallAI.TeleBot.Callback.Providers;

[CallbackQuery(OpenAICompatibleSettingsHandler.Tag)]
public class OpenAiPartCallback(IRepository<User, long> users, SettingsHandler<OpenAICompatibleSettingsState> handler)
    : SettingsPartCallback<OpenAICompatibleSettingsState>(users, handler);