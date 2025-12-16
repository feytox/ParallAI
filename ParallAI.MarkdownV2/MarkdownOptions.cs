namespace ParallAI.MarkdownV2;

public class MarkdownOptions
{
    /// <summary>
    /// Icon or text before heading level 1.
    /// Telegram doesn't support headings, so bold text with an icon is typically used.
    /// </summary>
    public string HeadLevel1 { get; set; } = "";

    public string HeadLevel2 { get; set; } = "";

    public string HeadLevel3 { get; set; } = "";

    public string HeadLevel4 { get; set; } = "";

    /// <summary>
    /// Symbol for completed task (- [x])
    /// </summary>
    public string TaskCompleted { get; set; } = "✅";

    /// <summary>
    /// Symbol for uncompleted task (- [ ])
    /// </summary>
    public string TaskUncompleted { get; set; } = "⬜";

    /// <summary>
    /// Icon for images when they are rendered as links
    /// </summary>
    public string Image { get; set; } = "🖼";

    /// <summary>
    /// If true, long quotes will be wrapped in spoiler ||...||
    /// </summary>
    public bool CiteExpandable { get; set; } = false;

    /// <summary>
    /// Default options
    /// </summary>
    public static MarkdownOptions Default => new();
}