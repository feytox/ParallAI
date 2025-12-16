using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramThematicBreakRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, ThematicBreakBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, ThematicBreakBlock obj)
    {
        renderer.WriteEscaped("───────────────────");
        renderer.Write("\n\n");
    }
}
