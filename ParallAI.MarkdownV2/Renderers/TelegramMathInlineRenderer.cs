using Markdig.Extensions.Mathematics;
using Markdig.Renderers;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramMathInlineRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, MathInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, MathInline obj)
    {
        renderer.WriteEscaped(obj.Content.ToString());
    }
}
