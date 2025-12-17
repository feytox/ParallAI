using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramQuoteRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, QuoteBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, QuoteBlock obj)
    {
        foreach (var block in obj)
        {
            renderer.Write(">");
            renderer.Render(block);
        }

        renderer.Write("\n\n");
    }
}
