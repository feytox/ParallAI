using System.Buffers;
using Markdig.Renderers;
using Markdig.Syntax;
using ParallAI.MarkdownV2.Renderers;

namespace ParallAI.MarkdownV2;

public class TelegramMarkdownRenderer : TextRendererBase<TelegramMarkdownRenderer>
{
    public MarkdownOptions Options { get; }

    private static readonly SearchValues<char> EscapableSearchValues =
        SearchValues.Create([
            '_', '*', '[', ']', '(', ')', '~', '>', '#', '+', '-', '=', '\\', '{', '}', '.', '!', '`'
        ]);

    public TelegramMarkdownRenderer(TextWriter writer, MarkdownOptions? options = null) : base(writer)
    {
        Options = options ?? MarkdownOptions.Default;

        ObjectRenderers.Add(new TelegramHeadingRenderer());
        ObjectRenderers.Add(new TelegramParagraphRenderer());
        ObjectRenderers.Add(new TelegramQuoteRenderer());
        ObjectRenderers.Add(new TelegramMathRenderer());
        ObjectRenderers.Add(new TelegramCodeBlockRenderer());
        ObjectRenderers.Add(new TelegramThematicBreakRenderer());
        ObjectRenderers.Add(new TelegramListRenderer());
        ObjectRenderers.Add(new TelegramTableRenderer());
        ObjectRenderers.Add(new TelegramLiteralInlineRenderer());
        ObjectRenderers.Add(new TelegramEmphasisRenderer());
        ObjectRenderers.Add(new TelegramLinkRenderer());
        ObjectRenderers.Add(new TelegramCodeInlineRenderer());
        ObjectRenderers.Add(new TelegramLineBreakRenderer());
        ObjectRenderers.Add(new TelegramMathInlineRenderer());
        ObjectRenderers.Add(new TelegramTaskListRenderer());
        ObjectRenderers.Add(new TelegramHtmlRenderer());
    }

    public TelegramMarkdownRenderer(MarkdownOptions? options = null) : this(new StringWriter(), options)
    {
    }

    public override string ToString() => (Writer as StringWriter)?.ToString() ?? "";

    public void Render(MarkdownObject document) => Write(document);

    public void WriteEscaped(string? text)
    {
        if (string.IsNullOrEmpty(text))
            return;

        foreach (var c in text)
        {
            if (EscapableSearchValues.Contains(c))
                Write('\\');
            Write(c);
        }
    }

    public static string EscapeUrl(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return "";

        return url.Replace("\\", @"\\").Replace(")", "\\)");
    }

    public static string EscapeCodeContent(string content) =>
        content.Replace("\\", @"\\").Replace("`", "\\`");

    public void WriteCodeBlock(string code, string language = "")
    {
        Write("```");
        if (!string.IsNullOrEmpty(language))
            Write(language);
        Write("\n");

        Write(EscapeCodeContent(code));

        if (!code.EndsWith("\n"))
            Write("\n");
        Write("```\n\n");
    }
    
    public void TrimEnd(int count)
    {
        if (Writer is not StringWriter stringWriter) return;
        
        var sb = stringWriter.GetStringBuilder();
        if (sb.Length >= count)
        {
            sb.Length -= count;
        }
    }
}
