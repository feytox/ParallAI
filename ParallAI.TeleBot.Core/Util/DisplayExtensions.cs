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
    private const string SelfHostInfo = "Боитесь за безопасность API-ключа? " +
                                        "<a href='https://github.com/feytox/ParallAI'><b>Захостите</b></a> бота сами!";

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
        return $"🔑 API-ключ: {state.Token.ToMaskedDisplay()}\n\n" +
               $"{SelfHostInfo}";
    }

    public static string ToFormattedString(this OpenRouterProviderSettingsState state)
    {
        return $"🔑 API-ключ: {state.Token.ToMaskedDisplay()}\n\n" +
               $"{SelfHostInfo}";
    }

    public static string ToFormattedString(this OpenAICompatibleSettingsState state)
    {
        var endpoint = state.Endpoint?.AbsoluteUri;
        return $"🔑 API-ключ: {state.Token.ToMaskedDisplay()}\n" +
               $"🌐 Endpoint Url: {endpoint.ToDisplay(maxLength: 60)}\n\n" +
               $"{SelfHostInfo}";
    }

    private static string ToMaskedDisplay(this string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return EmptyPlaceholder;

        if (value.Length < 10)
            return "...";

        return $"{value[..3]}...{value[^4..]}";
    }

    public static IEnumerable<string> SplitMessages(this string message, int limit = 4096)
    {
        if (message.Length <= limit)
        {
            yield return message;
            yield break;
        }

        var lines = message.Split('\n');
        var chunk = new StringBuilder();

        foreach (var line in lines)
        {
            var newLength = chunk.Length + line.Length + (chunk.Length > 0 ? 1 : 0);

            if (newLength <= limit)
            {
                if (chunk.Length > 0) chunk.Append('\n');
                chunk.Append(line);
                continue;
            }

            if (chunk.Length > 0)
            {
                yield return chunk.ToString();
                chunk.Clear();
            }

            if (line.Length > limit)
                foreach (var sub in SplitOversizedChunk(line, limit))
                    yield return sub;
            else
                chunk.Append(line);
        }

        if (chunk.Length > 0)
            yield return chunk.ToString();
    }

    private static IEnumerable<string> SplitOversizedChunk(string text, int limit)
    {
        for (var i = 0; i < text.Length; i += limit)
            yield return text.Substring(i, Math.Min(limit, text.Length - i));
    }
}