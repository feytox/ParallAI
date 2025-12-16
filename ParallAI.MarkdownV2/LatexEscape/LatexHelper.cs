using System.Text;

namespace ParallAI.MarkdownV2.LatexEscape;

public static class LatexHelper
{
    public static string ConvertToUnicode(string latex)
    {
        if (string.IsNullOrWhiteSpace(latex)) return "";
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
        var len = latex.Length;

        while (i < len)
        {
            var c = latex[i];

            if (c == '\\')
            {
                var (command, nextI) = ParseCommandManual(latex, i);
                var (handledStr, afterCmdIndex) = HandleCommand(command, latex, nextI);

                if (command == "\\frac" && result.Length > 0 && char.IsDigit(result[^1]) &&
                    handledStr.Length > 0 && char.IsDigit(handledStr[0]))
                {
                    result.Append(' ');
                }

                result.Append(handledStr);
                i = afterCmdIndex;
            }

            else if (c == '_' || c == '^')
            {
                var isSub = c == '_';

                if (isSub)
                {
                    bool isStartOfWord = (i == 0) || char.IsWhiteSpace(latex[i - 1]);
                    bool isNextEmpty = (i + 1 >= len) || char.IsWhiteSpace(latex[i + 1]);
                    if (isStartOfWord || isNextEmpty)
                    {
                        result.Append(c);
                        i++;
                        continue;
                    }
                }

                var nextI = i + 1;
                string arg;

                if (nextI < len && latex[nextI] == '{')
                {
                    var (blockContent, blockEnd) = ParseBlock(latex, nextI);
                    arg = blockContent;
                    nextI = blockEnd;
                }
                else if (nextI < len)
                {
                    if (latex[nextI] == '\\')
                    {
                        var (cmd, cmdEnd) = ParseCommandManual(latex, nextI);
                        var (val, _) = HandleCommand(cmd, latex, cmdEnd);
                        arg = val;
                        nextI = cmdEnd;
                    }
                    else
                    {
                        arg = latex[nextI].ToString();
                        nextI++;
                    }
                }
                else
                {
                    arg = "";
                }

                result.Append(isSub ? MakeSubscript(arg) : MakeSuperscript(arg));
                i = nextI;
            }
            else
            {
                result.Append(c);
                i++;
            }
        }

        return result.ToString();
    }

    private static (string Command, int NextIndex) ParseCommandManual(string latex, int start)
    {
        int i = start + 1;
        int len = latex.Length;
        if (i >= len) return ("\\", len);

        char first = latex[i];
        if (!char.IsLetter(first))
        {
            return (latex.Substring(start, 2), i + 1);
        }

        while (i < len && char.IsLetter(latex[i]))
        {
            i++;
        }

        return (latex.Substring(start, i - start), i);
    }

    private static (string Content, int NextIndex) ParseBlock(string latex, int start)
    {
        while (start < latex.Length && char.IsWhiteSpace(latex[start])) start++;

        if (start >= latex.Length || latex[start] != '{')
        {
            if (start < latex.Length)
            {
                if (latex[start] == '\\')
                {
                    var (cmd, end) = ParseCommandManual(latex, start);
                    var (res, next) = HandleCommand(cmd, latex, end);
                    return (res, next);
                }

                return (latex[start].ToString(), start + 1);
            }

            return ("", start);
        }

        var level = 1;
        var pos = start + 1;

        while (pos < latex.Length && level > 0)
        {
            if (latex[pos] == '\\')
            {
                pos += 2;
                continue;
            }

            if (latex[pos] == '{') level++;
            else if (latex[pos] == '}') level--;

            pos++;
        }

        int contentEnd = pos - 1;
        if (level > 0)
            contentEnd = pos;

        int contentStart = start + 1;
        int length = contentEnd - contentStart;

        if (length > 0)
        {
            var rawContent = latex.Substring(contentStart, length);
            var convertedContent = Parse(rawContent);
            return (convertedContent, pos);
        }

        return ("", pos);
    }

    private static (string Result, int NextIndex) HandleCommand(string command, string latex, int index)
    {
        if (LatexDefinitions.LatexSymbols.TryGetValue(command, out var symbol)) return (symbol, index);

        switch (command)
        {
            case "\\binom":
            case "\\tbinom":
            case "\\dbinom":
                var (n, idx1) = ParseBlock(latex, index);
                var (k, idx2) = ParseBlock(latex, idx1);
                return ($"C({n}, {k})", idx2);
            case "\\frac":
                var (numer, i1) = ParseBlock(latex, index);
                var (denom, i2) = ParseBlock(latex, i1);
                return (MakeFraction(numer, denom), i2);
            case "\\sqrt":
                int cur = index;
                string deg = "";
                if (cur < latex.Length && latex[cur] == '[')
                {
                    int close = latex.IndexOf(']', cur);
                    if (close != -1)
                    {
                        deg = latex.Substring(cur + 1, close - cur - 1);
                        cur = close + 1;
                    }
                }

                var (p, end) = ParseBlock(latex, cur);
                return (MakeSqrt(deg, p), end);
            case "\\text":
            case "\\mathrm":
                return ParseBlock(latex, index);
            case "\\left":
            case "\\right":
                return ("", index);
            case "\\":
            case "\\\\":
                return ("\n", index);
        }

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
        {
            string charToNegate;
            int nextI;
            int tempIdx = index;
            while (tempIdx < latex.Length && char.IsWhiteSpace(latex[tempIdx])) tempIdx++;
            if (tempIdx < latex.Length && latex[tempIdx] == '\\')
            {
                var (nextCmd, cmdEnd) = ParseCommandManual(latex, tempIdx);
                charToNegate = LatexDefinitions.LatexSymbols.TryGetValue(nextCmd, out var sym) ? sym : "";
                nextI = cmdEnd;
            }
            else if (tempIdx < latex.Length)
            {
                charToNegate = latex[tempIdx].ToString();
                nextI = tempIdx + 1;
            }
            else return ("", index);

            return (MakeNot(charToNegate), nextI);
        }

        var sb = new StringBuilder(command);
        var currentIdx = index;
        while (true)
        {
            int peekIdx = currentIdx;
            while (peekIdx < latex.Length && char.IsWhiteSpace(latex[peekIdx])) peekIdx++;
            if (peekIdx < latex.Length && latex[peekIdx] == '{')
            {
                var (blockContent, nextIdx) = ParseBlock(latex, currentIdx);
                sb.Append(blockContent);
                currentIdx = nextIdx;
            }
            else break;
        }

        return (sb.ToString(), currentIdx);
    }

    private static string MakeNot(string s) => LatexDefinitions.NotMap.TryGetValue(s, out var v) ? v : s + "\u0338";

    private static string MakeFraction(string n, string d)
    {
        n = n.Trim();
        d = d.Trim();
        return LatexDefinitions.FracMap.TryGetValue((n, d), out var f)
            ? f
            : $"{MaybeParenthesize(n)}/{MaybeParenthesize(d)}";
    }

    private static string MakeSqrt(string d, string r) =>
        (d switch { "3" => "∛", "4" => "∜", "" or "2" => "√", _ => $"^{d}√" }) + $"({r})";

    private static string MaybeParenthesize(string t) =>
        t.All(c => char.IsLetterOrDigit(c) || IsCombiningChar(c)) ? t : $"({t})";

    private static string TranslateStyles(string c, string t)
    {
        if (!LatexDefinitions.LatexStyles.TryGetValue(c, out var m)) return t;
        var sb = new StringBuilder();
        foreach (var ch in t) sb.Append(m.TryGetValue(ch.ToString(), out var r) ? r : ch);
        return sb.ToString();
    }

    private static string TranslateCombining(string c, string t)
    {
        if (!LatexDefinitions.Combining.TryGetValue(c, out var i)) return t;
        if (string.IsNullOrEmpty(t)) return i.Char;
        return i.Type switch
        {
            CombiningType.FirstChar => t.Insert(1, i.Char),
            CombiningType.LastChar => t + i.Char,
            CombiningType.EveryChar => string.Join("", t.Select(x => x + i.Char)),
            _ => t
        };
    }

    private static string MakeSubscript(string t) => MapCharsLocal(t, LatexDefinitions.Subscripts) ?? $"_({t})";
    private static string MakeSuperscript(string t) => MapCharsLocal(t, LatexDefinitions.Superscripts) ?? $"^({t})";

    private static string? MapCharsLocal(string text, IReadOnlyDictionary<string, string> primaryMap)
    {
        var sb = new StringBuilder();
        foreach (var c in text)
        {
            if (primaryMap.TryGetValue(c.ToString(), out var val)) sb.Append(val);
            else return null;
        }

        return sb.ToString();
    }

    private static bool IsCombiningChar(char c) => c is >= '\u0300' and <= '\u036F' or >= '\u20D0' and <= '\u20FF';
}