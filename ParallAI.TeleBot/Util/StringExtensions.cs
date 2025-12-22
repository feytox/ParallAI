namespace ParallAI.TeleBot.Util;

internal static class StringExtensions
{
    public static string? ParseAscii(this string text)
    {
        var trimmed = text.Trim();
        return trimmed.All(c => c < 128) ? trimmed : null;
    }
}