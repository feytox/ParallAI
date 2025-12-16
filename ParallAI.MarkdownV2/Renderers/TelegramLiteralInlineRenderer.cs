using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramLiteralInlineRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, LiteralInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, LiteralInline obj)
    {
        renderer.WriteEscaped(obj.Content.ToString());
    }
}
