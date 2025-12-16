using Markdig.Renderers;
using Markdig.Syntax;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramListRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, ListBlock>
{
    protected override void Write(TelegramMarkdownRenderer renderer, ListBlock obj)
    {
        for (var i = 0; i < obj.Count; i++)
        {
            if (obj[i] is not ListItemBlock listItem)
                continue;

            WriteListItemBullet(renderer, obj.IsOrdered, i + 1);
            renderer.WriteChildren(listItem);
        }

        renderer.Write("\n");
    }

    private static void WriteListItemBullet(TelegramMarkdownRenderer renderer, bool isOrdered, int number)
    {
        if (isOrdered)
        {
            renderer.Write($"{number}\\. ");
        }
        else
        {
            renderer.WriteEscaped("•");
            renderer.Write(" ");
        }
    }
}
