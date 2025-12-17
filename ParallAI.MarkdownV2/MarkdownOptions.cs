namespace ParallAI.MarkdownV2;

public class MarkdownOptions
{
    /// <summary>
    /// Icon or text before heading level 1.
    /// Telegram doesn't support headings, so bold text with an icon is typically used.
    /// </summary>
    public string HeadLevel1 { get; init; } = "";

    public string HeadLevel2 { get; init; } = "";

    public string HeadLevel3 { get; init; } = "";

    public string HeadLevel4 { get; init; } = "";
    
    public string HeadLevel5 { get; init; } = "";

    public string HeadLevel6 { get; init; } = "";

    /// <summary>
    /// Symbol for completed task (- [x])
    /// </summary>
    public string TaskCompleted { get; init; } = "✅";

    /// <summary>
    /// Symbol for uncompleted task (- [ ])
    /// </summary>
    public string TaskUncompleted { get; init; } = "⬜";

    /// <summary>
    /// Default options
    /// </summary>
    public static MarkdownOptions Default => new();
}