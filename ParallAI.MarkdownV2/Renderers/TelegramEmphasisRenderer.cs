using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramEmphasisRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, EmphasisInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, EmphasisInline obj)
    {
        var tag = GetEmphasisTag(obj);
        renderer.Write(tag);
        renderer.WriteChildren(obj);
        renderer.Write(tag);
    }

    private static string GetEmphasisTag(EmphasisInline emphasis)
    {
        return emphasis.DelimiterChar switch
        {
            '*' or '_' => emphasis.DelimiterCount == 2 ? "*" : "_",
            '~' => "~",
            _ => ""
        };
    }
}
