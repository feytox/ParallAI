using Markdig.Extensions.Mathematics;
using Markdig.Renderers;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramMathRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, MathBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, MathBlock obj)
    {
        foreach (var line in obj.Lines)
        {
            renderer.WriteEscaped(line.ToString());
            renderer.Write("\n");
        }

        renderer.Write("\n");
    }
}
