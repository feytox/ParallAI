namespace ParallAI.MarkdownV2;

public class MarkdownOptions
{
    /// <summary>
    /// Иконка или текст перед заголовком 1 уровня.
    /// В Telegram нет заголовков, поэтому обычно используют жирный текст + иконку.
    /// </summary>
    public string HeadLevel1 { get; set; } = "";

    public string HeadLevel2 { get; set; } = "";

    public string HeadLevel3 { get; set; } = "";

    public string HeadLevel4 { get; set; } = "";

    /// <summary>
    /// Символ для выполненной задачи (- [x])
    /// </summary>
    public string TaskCompleted { get; set; } = "✅";

    /// <summary>
    /// Символ для невыполненной задачи (- [ ])
    /// </summary>
    public string TaskUncompleted { get; set; } = "⬜";

    /// <summary>
    /// Иконка для обозначения картинок, если они идут ссылкой
    /// </summary>
    public string Image { get; set; } = "🖼";

    /// <summary>
    /// Если true, длинные цитаты будут оборачиваться в спойлер ||...||
    /// </summary>
    public bool CiteExpandable { get; set; } = false;

    /// <summary>
    /// Стандартные настройки
    /// </summary>
    public static MarkdownOptions Default => new MarkdownOptions();
}