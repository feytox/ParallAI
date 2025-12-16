using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramParagraphRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, ParagraphBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, ParagraphBlock obj)
    {
        if (obj.Inline != null)
            renderer.WriteChildren(obj.Inline);

        if (obj.Parent is ListItemBlock)
        {
            renderer.Write("\n");
            return;
        }

        var nextIsList = IsNextSiblingList(obj);
        renderer.Write(nextIsList ? "\n" : "\n\n");
    }

    private static bool IsNextSiblingList(ParagraphBlock paragraph)
    {
        if (paragraph.Parent is not { } parentContainer)
            return false;

        var index = parentContainer.IndexOf(paragraph);
        if (index < 0 || index >= parentContainer.Count - 1)
            return false;

        return parentContainer[index + 1] is ListBlock;
    }
}
