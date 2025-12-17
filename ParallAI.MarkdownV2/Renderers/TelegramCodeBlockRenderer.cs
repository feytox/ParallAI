using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramCodeBlockRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, CodeBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, CodeBlock obj)
    {
        var lang = (obj as FencedCodeBlock)?.Info ?? "";
        var code = obj.Lines.ToString();
        renderer.WriteCodeBlock(code, lang);
    }
}
