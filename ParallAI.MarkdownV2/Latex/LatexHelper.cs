using System.Text;

namespace ParallAI.MarkdownV2.Latex;

public static class LatexHelper
{
    public static string ConvertToUnicode(string latex)
    {
        if (string.IsNullOrWhiteSpace(latex))
            return string.Empty;
        try
        {
            return Parse(latex);
        }
        catch
        {
            return latex;
        }
    }

    private static string Parse(string latex)
    {
        var result = new StringBuilder();
        var i = 0;

        while (i < latex.Length)
        {
            switch (latex[i])
            {
                case '\\':
                    i = HandleBackslash(latex, i, result);
                    break;
                case '_':
                case '^':
                    i = HandleScriptChar(latex, i, result);
                    break;
                default:
                    result.Append(latex[i]);
                    i++;
                    break;
            }
        }

        return result.ToString();
    }

    private static int HandleBackslash(string latex, int i, StringBuilder result)
    {
        var (command, nextI) = ParseCommand(latex, i);
        var (handledStr, afterCmdIndex) = HandleCommand(command, latex, nextI);

        if (command == "\\frac" && NeedsFractionSpace(result, handledStr))
            result.Append(' ');

        result.Append(handledStr);
        return afterCmdIndex;
    }

    private static bool NeedsFractionSpace(StringBuilder result, string handledStr) =>
        result.Length > 0 && char.IsDigit(result[^1]) &&
        handledStr.Length > 0 && char.IsDigit(handledStr[0]);

    private static int HandleScriptChar(string latex, int i, StringBuilder result)
    {
        var isSub = latex[i] == '_';

        if (isSub && IsUnderscoreAsLiteral(latex, i))
        {
            result.Append('_');
            return i + 1;
        }

        var (arg, nextI) = ParseScriptArgument(latex, i + 1);
        result.Append(isSub ? MakeSubscript(arg) : MakeSuperscript(arg));
        return nextI;
    }

    private static bool IsUnderscoreAsLiteral(string latex, int i)
    {
        var isStartOfWord = i == 0 || char.IsWhiteSpace(latex[i - 1]);
        var isNextEmpty = i + 1 >= latex.Length || char.IsWhiteSpace(latex[i + 1]);
        return isStartOfWord || isNextEmpty;
    }

    private static (string Arg, int NextIndex) ParseScriptArgument(string latex, int start)
    {
        if (start >= latex.Length)
            return ("", start);

        switch (latex[start])
        {
            case '{':
                return ParseBlock(latex, start);
            case '\\':
                var (cmd, cmdEnd) = ParseCommand(latex, start);
                var (val, _) = HandleCommand(cmd, latex, cmdEnd);
                return (val, cmdEnd);
            default:
                return (latex[start].ToString(), start + 1);
        }
    }

    private static (string Command, int NextIndex) ParseCommand(string latex, int start)
    {
        var i = start + 1;
        if (i >= latex.Length)
            return ("\\", latex.Length);

        if (!char.IsLetter(latex[i]))
            return (latex.Substring(start, 2), i + 1);

        while (i < latex.Length && char.IsLetter(latex[i]))
            i++;

        return (latex.Substring(start, i - start), i);
    }

    private static (string Content, int NextIndex) ParseBlock(string latex, int start)
    {
        start = SkipWhitespace(latex, start);

        if (start >= latex.Length)
            return ("", start);

        return latex[start] != '{' 
            ? ParseNonBracedBlock(latex, start) 
            : ParseBracedContent(latex, start);
    }

    private static (string Content, int NextIndex) ParseNonBracedBlock(string latex, int start)
    {
        if (latex[start] != '\\') 
            return (latex[start].ToString(), start + 1);
        
        var (cmd, end) = ParseCommand(latex, start);
        return HandleCommand(cmd, latex, end);
    }

    private static (string Content, int NextIndex) ParseBracedContent(string latex, int start)
    {
        var level = 1;
        var pos = start + 1;

        while (pos < latex.Length && level > 0)
        {
            switch (latex[pos])
            {
                case '\\':
                    pos += 2;
                    continue;
                case '{':
                    level++;
                    break;
                case '}':
                    level--;
                    break;
            }

            pos++;
        }

        var contentEnd = level > 0 ? pos : pos - 1;
        var contentStart = start + 1;
        var length = contentEnd - contentStart;

        if (length <= 0)
            return ("", pos);

        var rawContent = latex.Substring(contentStart, length);
        return (Parse(rawContent), pos);
    }

    private static int SkipWhitespace(string latex, int pos)
    {
        while (pos < latex.Length && char.IsWhiteSpace(latex[pos]))
            pos++;
        return pos;
    }

    private static (string Result, int NextIndex) HandleCommand(string command, string latex, int index)
    {
        if (LatexDefinitions.LatexSymbols.TryGetValue(command, out var symbol))
            return (symbol, index);

        return command switch
        {
            "\\binom" or "\\tbinom" or "\\dbinom" => HandleBinom(latex, index),
            "\\frac" => HandleFrac(latex, index),
            "\\sqrt" => HandleSqrt(latex, index),
            "\\text" or "\\mathrm" => ParseBlock(latex, index),
            "\\left" or "\\right" => ("", index),
            "\\" or @"\\" => ("\n", index),
            _ => HandleSpecialOrUnknown(command, latex, index)
        };
    }

    private static (string Result, int NextIndex) HandleBinom(string latex, int index)
    {
        var (n, idx1) = ParseBlock(latex, index);
        var (k, idx2) = ParseBlock(latex, idx1);
        return ($"C({n}, {k})", idx2);
    }

    private static (string Result, int NextIndex) HandleFrac(string latex, int index)
    {
        var (numerator, i1) = ParseBlock(latex, index);
        var (denominator, i2) = ParseBlock(latex, i1);
        return (MakeFraction(numerator, denominator), i2);
    }

    private static (string Result, int NextIndex) HandleSqrt(string latex, int index)
    {
        var deg = "";
        if (index < latex.Length && latex[index] == '[')
        {
            var close = latex.IndexOf(']', index);
            if (close != -1)
            {
                deg = latex.Substring(index + 1, close - index - 1);
                index = close + 1;
            }
        }

        var (content, end) = ParseBlock(latex, index);
        return (MakeSqrt(deg, content), end);
    }

    private static (string Result, int NextIndex) HandleSpecialOrUnknown(string command, string latex, int index)
    {
        if (LatexDefinitions.Combining.ContainsKey(command))
        {
            var (arg, newI) = ParseBlock(latex, index);
            return (TranslateCombining(command, arg), newI);
        }

        if (LatexDefinitions.LatexStyles.ContainsKey(command))
        {
            var (txt, newI) = ParseBlock(latex, index);
            return (TranslateStyles(command, txt), newI);
        }

        if (LatexDefinitions.NotMap.ContainsKey(command) || command == "\\not")
            return HandleNot(latex, index);

        return HandleUnknownCommand(command, latex, index);
    }

    private static (string Result, int NextIndex) HandleNot(string latex, int index)
    {
        var tempIdx = SkipWhitespace(latex, index);

        if (tempIdx >= latex.Length)
            return ("", index);

        string charToNegate;
        int nextI;

        if (latex[tempIdx] == '\\')
        {
            var (nextCmd, cmdEnd) = ParseCommand(latex, tempIdx);
            charToNegate = LatexDefinitions.LatexSymbols.GetValueOrDefault(nextCmd, "");
            nextI = cmdEnd;
        }
        else
        {
            charToNegate = latex[tempIdx].ToString();
            nextI = tempIdx + 1;
        }

        return (MakeNot(charToNegate), nextI);
    }

    private static (string Result, int NextIndex) HandleUnknownCommand(string command, string latex, int index)
    {
        var sb = new StringBuilder(command);

        while (true)
        {
            var peekIdx = SkipWhitespace(latex, index);
            if (peekIdx < latex.Length && latex[peekIdx] == '{')
            {
                var (blockContent, nextIdx) = ParseBlock(latex, index);
                sb.Append(blockContent);
                index = nextIdx;
            }
            else
                break;
        }

        return (sb.ToString(), index);
    }

    private static string MakeNot(string s) =>
        LatexDefinitions.NotMap.TryGetValue(s, out var v) ? v : s + "\u0338";

    private static string MakeFraction(string numerator, string denominator)
    {
        numerator = numerator.Trim();
        denominator = denominator.Trim();
        return LatexDefinitions.FracMap.TryGetValue((numerator, denominator), out var frac)
            ? frac
            : $"{MaybeParenthesize(numerator)}/{MaybeParenthesize(denominator)}";
    }

    private static string MakeSqrt(string d, string r) =>
        (d switch { "3" => "∛", "4" => "∜", "" or "2" => "√", _ => $"^{d}√" }) + $"({r})";

    private static string MaybeParenthesize(string t) =>
        t.All(c => char.IsLetterOrDigit(c) || IsCombiningChar(c)) ? t : $"({t})";

    private static string TranslateStyles(string c, string t)
    {
        if (!LatexDefinitions.LatexStyles.TryGetValue(c, out var m))
            return t;

        var sb = new StringBuilder();
        foreach (var ch in t)
            sb.Append(m.TryGetValue(ch.ToString(), out var r) ? r : ch);

        return sb.ToString();
    }

    private static string TranslateCombining(string c, string t)
    {
        if (!LatexDefinitions.Combining.TryGetValue(c, out var i))
            return t;

        if (string.IsNullOrEmpty(t))
            return i.Char;

        return i.Type switch
        {
            CombiningType.FirstChar => t.Insert(1, i.Char),
            CombiningType.LastChar => t + i.Char,
            CombiningType.EveryChar => string.Join("", t.Select(x => x + i.Char)),
            _ => t
        };
    }

    private static string MakeSubscript(string t) =>
        MapChars(t, LatexDefinitions.Subscripts) ?? $"_({t})";

    private static string MakeSuperscript(string t) =>
        MapChars(t, LatexDefinitions.Superscripts) ?? $"^({t})";

    private static string? MapChars(string text, IReadOnlyDictionary<string, string> map)
    {
        var sb = new StringBuilder();
        foreach (var c in text)
        {
            if (map.TryGetValue(c.ToString(), out var val))
                sb.Append(val);
            else
                return null;
        }

        return sb.ToString();
    }

    private static bool IsCombiningChar(char c) =>
        c is >= '\u0300' and <= '\u036F' or >= '\u20D0' and <= '\u20FF';
}