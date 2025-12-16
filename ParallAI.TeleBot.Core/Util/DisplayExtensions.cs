using System.Text;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Core.States;
using ParallAI.Core.States.Providers;
using ParallAI.Core.ValueTypes;

namespace ParallAI.TeleBot.Core.Util;

public static class DisplayExtensions
{
    private const string EmptyPlaceholder = "(не задано)";
    
    public static string ToDisplay(this string? value, int maxLength = 0, string emptyPlaceholder = EmptyPlaceholder)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            return emptyPlaceholder;

        if (maxLength > 0 && value.Length > maxLength)
            return value[..maxLength] + $"... (показаны первые {maxLength} символов)";

        return value;
    }
    
    public static string ToFormattedString(this PresetSettingsState state)
    {
        var sb = new StringBuilder();
        var temperatureText = state.Temperature.HasValue ? state.Temperature.Value.ToString("0.0") : EmptyPlaceholder;
        sb.AppendLine($"🌡 Температура: {temperatureText}");
    
        var budget = state.ThinkingBudget == ThinkingBudget.Unknown 
            ? EmptyPlaceholder
            : state.ThinkingBudget.ToString();
        sb.AppendLine($"🧠 Бюджет: {budget}");
        sb.AppendLine($"💬 Промпт: {state.SystemPrompt.ToDisplay(maxLength: 300)}");

        return sb.ToString();
    }

    public static string ToFormattedString(this Preset preset) => ToFormattedString(preset.ToState());
    
    public static string ToFormattedString(this ModelSettingsState state)
    {
        var sb = new StringBuilder();
        sb.AppendLine();
        sb.AppendLine($"🆔 ID модели: {state.ModelId.ToDisplay(maxLength: 300)}");
        
        var providerName = state.Provider switch
        {
            null => EmptyPlaceholder,
            GeminiProvider => "Google Gemini", 
            OpenRouterProvider => "OpenRouter",
            OpenAICompatibleProvider => "OpenAI совместимый",
            _ => state.Provider.GetType().Name
        };
    
        sb.AppendLine($"🔌 Провайдер: {providerName}");

        return sb.ToString();
    }
    
    public static string ToFormattedString(this AiModel model) => ToFormattedString(model.ToState());
    
    public static string ToFormattedString(this GeminiProviderSettingsState state)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"🔑 Токен: {state.Token.ToMaskedDisplay()}");
        return sb.ToString();
    }
    
    public static string ToFormattedString(this OpenRouterProviderSettingsState state)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"🔑 Токен: {state.Token.ToMaskedDisplay()}");
        return sb.ToString();
    }
    
    public static string ToFormattedString(this OpenAICompatibleSettingsState state)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"🔑 Токен: {state.Token.ToMaskedDisplay()}");
        sb.AppendLine($"🌐 Endpoint Url: {state.Endpoint?.AbsoluteUri.ToDisplay(maxLength: 60)}");
        return sb.ToString();
    }
    
    private static string ToMaskedDisplay(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value)) 
            return EmptyPlaceholder;

        if (value.Length < 10) 
            return "...";
        
        return $"{value[..3]}...{value[^4..]}";
    }
}