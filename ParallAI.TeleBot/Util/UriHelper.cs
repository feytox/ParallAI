using System.Text.RegularExpressions;
using ParallAI.Core.ValueTypes;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ParallAI.TeleBot.Util;

public static partial class UriHelper
{
    
    [GeneratedRegex("""^(?:https?:\/\/)?[\p{L}0-9-]+(?:\.[\p{L}0-9-]+)+\.?(?:\:[0-9]{1,5})?(?:\/[^\s]*)?$""")]
    private static partial Regex HttpUriRegex();

    public static bool TryCreateHttp(string? uriString, out Uri? result)
    {
        if (string.IsNullOrEmpty(uriString) || !HttpUriRegex().IsMatch(uriString))
        {
            result = null;
            return false;
        }

        if (!uriString.StartsWith("http://") && !uriString.StartsWith("https://"))
            uriString = "https://" + uriString;

        result = new Uri(uriString, UriKind.Absolute);
        return true;
    }
}