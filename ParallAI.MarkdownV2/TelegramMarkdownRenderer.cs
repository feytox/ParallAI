using System.Buffers;
using System.Text;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdig.Extensions.Tables;
using Markdig.Extensions.TaskLists;
using Markdig.Extensions.Mathematics;

namespace ParallAI.MarkdownV2;

public class TelegramMarkdownRenderer : TextRendererBase<TelegramMarkdownRenderer>
{
    private readonly MarkdownOptions _options;
    
    private static readonly SearchValues<char> EscapableSearchValues = 
        SearchValues.Create(['_', '*', '[', ']', '(', ')', '~', '>', '#', '+', '-', '=', '\\', '{', '}', '.', '!', '`']);

    private void WriteEscaped(string? text)
    {
        if (string.IsNullOrEmpty(text)) return;

        foreach (var c in text)
        {
            if (EscapableSearchValues.Contains(c)) 
            {
                Write('\\');
            }
            Write(c);
        }
    }

    public TelegramMarkdownRenderer(TextWriter writer, MarkdownOptions? options = null) : base(writer)
    {
        _options = options ?? MarkdownOptions.Default;
    }
    
    public TelegramMarkdownRenderer(MarkdownOptions? options = null) : base(new StringWriter())
    {
        _options = options ?? MarkdownOptions.Default;
    }

    public override string ToString()
    {
        return (Writer as StringWriter)?.ToString() ?? "";
    }

    public void Render(MarkdownObject document)
    {
        RenderNode(document);
    }

    private void RenderNode(MarkdownObject obj)
    {
        switch (obj)
        {

            case HeadingBlock heading:
                Write("\n");
                string icon = heading.Level switch
                {
                    1 => _options.HeadLevel1,
                    2 => _options.HeadLevel2,
                    3 => _options.HeadLevel3,
                    _ => _options.HeadLevel4
                };
                if (!string.IsNullOrEmpty(icon)) Write(icon + " ");
                
                Write("*");
                if (heading.Inline != null) WriteChildren(heading.Inline);
                Write("*\n\n");
                break;

            case ParagraphBlock paragraph:
                if (paragraph.Inline != null) WriteChildren(paragraph.Inline);

                if (paragraph.Parent is ListItemBlock)
                {
                    Write("\n");
                }
                else
                {
                    bool nextIsList = false;
                    if (paragraph.Parent is ContainerBlock parentContainer)
                    {
                        var index = parentContainer.IndexOf(paragraph);
                        if (index >= 0 && index < parentContainer.Count - 1)
                        {
                            if (parentContainer[index + 1] is ListBlock)
                            {
                                nextIsList = true;
                            }
                        }
                    }

                    if (nextIsList) Write("\n");
                    else Write("\n\n");
                }
                break;

            case QuoteBlock quoteBlock:
                foreach (var block in quoteBlock)
                {
                    Write(">"); 
                    if (_options.CiteExpandable) Write(" ");
                    RenderNode(block); 
                }
                Write("\n\n");
                break;

            
            case MathBlock mathBlock:
                foreach(var line in mathBlock.Lines)
                {
                    // line.ToString() вернет содержимое строки
                    WriteEscaped(line.ToString());
                    Write("\n");
                }
                Write("\n");
                break;

            case FencedCodeBlock fencedCode:
                WriteCodeBlock(fencedCode.Info, fencedCode.Lines.ToString());
                break;

            case CodeBlock codeBlock:
                WriteCodeBlock("", codeBlock.Lines.ToString());
                break;

            case ThematicBreakBlock:
                WriteEscaped("───────────────────");
                Write("\n\n");
                break;
            
            case ListBlock listBlock:
                WriteList(listBlock);
                break;

            case Table table:
                WriteTableAsCode(table);
                break;

            // --- INLINE ЭЛЕМЕНТЫ ---

            case LiteralInline literal:
                WriteEscaped(literal.Content.ToString());
                break;

            case EmphasisInline emphasis:
                string tag = "";
                // Markdig: count 2 = bold (*), count 1 = italic (_)
                if (emphasis.DelimiterChar == '*' || emphasis.DelimiterChar == '_')
                {
                    tag = emphasis.DelimiterCount == 2 ? "*" : "_";
                }
                else if (emphasis.DelimiterChar == '~')
                {
                    tag = "~";
                }
                
                Write(tag);
                WriteChildren(emphasis);
                Write(tag);
                break;

            case LinkInline link:
                Write("[");
                WriteChildren(link);
                Write("](");
                Write(EscapeUrl(link.Url)); // <--- Метод экранирования URL
                Write(")");
                break;

            case CodeInline code:
                Write("`");
                Write(code.Content.ToString().Replace("\\", "\\\\").Replace("`", "\\`"));
                Write("`");
                break;

            case LineBreakInline:
                Write("\n");
                break;
            
            // MATH INLINE ($...$)
            case MathInline math:
                WriteEscaped(math.Content.ToString());
                break;
                 
            // EXTENSIONS
            case TaskList taskList:
                var symbol = taskList.Checked ? _options.TaskCompleted : _options.TaskUncompleted;
                Write(symbol + " ");
                break;
                
            case HtmlInline:
                // Игнорируем HTML
                break;
            
            case ContainerBlock container:
                WriteChildren(container);
                break;
                
            case ContainerInline containerInline:
                WriteChildren(containerInline);
                break;

            default:
                // Fallback для неизвестных блоков - пытаемся пройти вглубь
                WriteChildren(obj);
                break;
        }
    }

    // --- Helpers ---

    private void WriteChildren(MarkdownObject obj)
    {
        if (obj is ContainerBlock containerBlock)
        {
            foreach (var child in containerBlock) RenderNode(child);
        }
        else if (obj is ContainerInline containerInline)
        {
            foreach (var child in containerInline) RenderNode(child);
        }
        else if (obj is LeafBlock leafBlock && leafBlock.Inline != null)
        {
            foreach (var child in leafBlock.Inline) RenderNode(child);
        }
    }

    private void WriteCodeBlock(string? lang, string code)
    {
        Write("```");
        if (!string.IsNullOrEmpty(lang)) Write(lang);
        Write("\n");
        
        Write(code.Replace("\\", "\\\\").Replace("`", "\\`"));
        
        if (!code.EndsWith("\n")) Write("\n");
        Write("```\n\n");
    }

    private void WriteList(ListBlock list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            var item = list[i];
            string bullet;
            
            if (list.IsOrdered)
            {
                bullet = $"{i + 1}\\."; // Точку экранируем
            }
            else
            {
                bullet = "•"; // Буллит не экранируем, так как это не спецсимвол MarkdownV2
            }

            if (item is ListItemBlock listItem)
            {
                if (!list.IsOrdered) 
                {
                    WriteEscaped("•"); 
                    Write(" ");
                }
                else
                {
                    Write(bullet + " ");
                }
                
                // Рендерим содержимое элемента списка
                WriteChildren(listItem);
            }
        }
        Write("\n");
    }

    private void WriteTableAsCode(Table table)
    {
        var sb = new StringBuilder();
        foreach (var rowObj in table)
        {
            if (rowObj is TableRow row)
            {
                var cells = new List<string>();
                foreach (var cellObj in row)
                {
                    if (cellObj is TableCell cell)
                    {
                        var cellText = new StringBuilder();
                        // Собираем текст из ячейки (упрощенно)
                        foreach(var block in cell) 
                        {
                            if (block is ParagraphBlock pb && pb.Inline != null) 
                            {
                                foreach(var inline in pb.Inline)
                                {
                                    if(inline is LiteralInline lit) cellText.Append(lit.Content);
                                    else if(inline is CodeInline cod) cellText.Append(cod.Content);
                                    else cellText.Append(" ");
                                }
                            }
                        }
                        cells.Add(cellText.ToString());
                    }
                }
                sb.AppendLine(string.Join(" | ", cells));
            }
        }
        
        WriteCodeBlock("", sb.ToString());
    }
    
    private string EscapeUrl(string? url)
    {
        if (string.IsNullOrEmpty(url)) return "";
        // Telegram MarkdownV2 требует экранировать ')' и '\' внутри ссылки
        return url.Replace("\\", "\\\\").Replace(")", "\\)");
    }
}