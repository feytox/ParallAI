using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramLineBreakRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, LineBreakInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, LineBreakInline obj)
    {
        renderer.Write("\n");
    }
}
