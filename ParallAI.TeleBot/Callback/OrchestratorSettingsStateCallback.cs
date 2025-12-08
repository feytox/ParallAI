using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(OrchestratorSettingsHandler.CallbackTag)]
public class OrchestratorSettingsStateCallback(IRepository<User, long> users, OrchestratorSettingsHandler handler) 
    : StandardSettingsStateCallback<OrchestratorSettingsState, OrchestratorSettingsHandler>(users, handler);