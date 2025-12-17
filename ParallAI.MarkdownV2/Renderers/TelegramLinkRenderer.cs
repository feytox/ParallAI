using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramLinkRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, LinkInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, LinkInline obj)
    {
        renderer.Write("[");
        renderer.WriteChildren(obj);
        renderer.Write("](");
        renderer.Write(TelegramMarkdownRenderer.EscapeUrl(obj.Url));
        renderer.Write(")");
    }
}
