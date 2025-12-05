using ParallAI.Core.Entities;
using ParallAI.Core.Repositories;
using ParallAI.Core.States;
using ParallAI.TeleBot.Core.Callback.Common;
using ParallAI.TeleBot.Settings;

namespace ParallAI.TeleBot.Callback;

[CallbackQuery(CompareElementSettingsHandler.CallbackTag)]
public class CompareElementStateCallback(IRepository<User, long> users, CompareElementSettingsHandler handler)
    : StandardSettingsStateCallback<CompareElementSettingsState, CompareElementSettingsHandler>(users, handler);