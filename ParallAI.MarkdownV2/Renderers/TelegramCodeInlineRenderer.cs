using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramCodeInlineRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, CodeInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, CodeInline obj)
    {
        renderer.Write("`");
        renderer.Write(TelegramMarkdownRenderer.EscapeCodeContent(obj.Content));
        renderer.Write("`");
    }
}
