using System.Text;
using Markdig.Extensions.Tables;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramTableRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, Table>
{
    protected override void Write(TelegramMarkdownRenderer renderer, Table obj)
    {
        var sb = new StringBuilder();

        foreach (var rowObj in obj)
        {
            if (rowObj is not TableRow row)
                continue;

            var cells = ExtractRowCells(row);
            sb.AppendLine(string.Join(" | ", cells));
        }

        var code = sb.ToString();
        renderer.WriteCodeBlock(code);
    }

    private static List<string> ExtractRowCells(TableRow row)
    {
        var cells = new List<string>();

        foreach (var cellObj in row)
        {
            if (cellObj is not TableCell cell)
                continue;

            cells.Add(ExtractCellText(cell));
        }

        return cells;
    }

    private static string ExtractCellText(TableCell cell)
    {
        var cellText = new StringBuilder();

        foreach (var block in cell)
        {
            if (block is not ParagraphBlock { Inline: not null } pb)
                continue;

            foreach (var inline in pb.Inline)
            {
                var text = inline switch
                {
                    LiteralInline lit => lit.Content.ToString(),
                    CodeInline cod => cod.Content,
                    _ => " "
                };
                cellText.Append(text);
            }
        }

        return cellText.ToString();
    }
}
