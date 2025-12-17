using System.Text.RegularExpressions;

namespace ParallAI.MarkdownV2.Utils;

public static partial class UrlProtector
{
    [GeneratedRegex(@"(!?\[.*?\])\((.*?)\)", RegexOptions.Compiled)]
    private static partial Regex LinkRegex();

    public static (string MaskedText, Dictionary<string, string> Map) MaskUrls(string input)
    {
        var map = new Dictionary<string, string>();
        var counter = 0;

        var masked = LinkRegex().Replace(input, match =>
        {
            var prefix = match.Groups[1].Value;
            var urlContent = match.Groups[2].Value;
            var token = $"XURLTOKENX{counter++}X";
            map[token] = urlContent;
            return $"{prefix}({token})";
        });

        return (masked, map);
    }

    public static string RestoreUrls(string input, Dictionary<string, string> map)
    {
        return map.Aggregate(input, (current, kvp) => current.Replace(kvp.Key, kvp.Value));
    }
}
