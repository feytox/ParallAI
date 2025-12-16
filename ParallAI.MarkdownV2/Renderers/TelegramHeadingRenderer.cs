using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramHeadingRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, HeadingBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, HeadingBlock obj)
    {
        renderer.Write("\n");
        var icon = GetHeadingIcon(renderer.Options, obj.Level);
        if (!string.IsNullOrEmpty(icon))
            renderer.Write(icon + " ");

        renderer.Write("*");
        if (obj.Inline != null)
            renderer.WriteChildren(obj.Inline);
        renderer.Write("*\n\n");
    }

    private static string GetHeadingIcon(MarkdownOptions options, int level) =>
        level switch
        {
            1 => options.HeadLevel1,
            2 => options.HeadLevel2,
            3 => options.HeadLevel3,
            _ => options.HeadLevel4
        };
}
