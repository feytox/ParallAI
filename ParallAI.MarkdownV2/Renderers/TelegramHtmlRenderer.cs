using Markdig.Renderers;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramHtmlRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, HtmlInline>
{
    protected override void Write(TelegramMarkdownRenderer renderer, HtmlInline obj)
    {
    }
}
