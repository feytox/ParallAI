using System.Text.RegularExpressions;
using ParallAI.MarkdownV2.LatexEscape;

namespace ParallAI.MarkdownV2.Utils;

public static partial class MathProcessor
{
    [GeneratedRegex(@"(\$\$[\s\S]+?\$\$)|(\\\[[\s\S]+?\\\])|(\$[^$\n]+?\$)", RegexOptions.Compiled)]
    private static partial Regex MathRegex();

    public static string ProcessMathBlocks(string input) =>
        MathRegex().Replace(input, match => LatexHelper.ConvertToUnicode(ExtractMathContent(match.Value)));

    private static string ExtractMathContent(string rawMatch)
    {
        if (rawMatch.StartsWith("$$"))
            return rawMatch[2..^2];

        if (rawMatch.StartsWith("\\["))
            return rawMatch[2..^2];

        return rawMatch[1..^1];
    }
}
