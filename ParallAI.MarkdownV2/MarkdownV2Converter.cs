using System.Text.RegularExpressions;
using Markdig;
using ParallAI.MarkdownV2.LatexEscape;

namespace ParallAI.MarkdownV2;

public class MarkdownV2Converter
{
    private readonly MarkdownPipeline _pipeline;

    public MarkdownV2Converter()
    {
        _pipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()                 // Таблицы
            .UseTaskLists()                  // - [ ]
            .UseAutoLinks()                  // Авто-ссылки
            .UseEmphasisExtras()             // ~~зачеркнутый~~
            .UseMathematics()                // $...$
            .UseSoftlineBreakAsHardlineBreak() // Enter = новая строка
            .Build();
    }

    public string Convert(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        // 1. МАСКИРОВКА ССЫЛОК
        var (textWithMaskedUrls, urlMap) = MaskUrls(markdown);

        // 2. ОБРАБОТКА LATEX (Только внутри $...$, $$...$$, \[...\])
        // Теперь LatexHelper не трогает обычный текст, а только то, что явно выделено как формула.
        var textWithUnicodeMath = ProcessMathBlocks(textWithMaskedUrls);

        // 3. ВОССТАНОВЛЕНИЕ ССЫЛОК
        var cleanMarkdown = RestoreUrls(textWithUnicodeMath, urlMap);

        // 4. Парсинг Markdig
        var document = Markdown.Parse(cleanMarkdown, _pipeline);

        // 5. Рендеринг в Telegram MarkdownV2
        using var writer = new StringWriter();
        var renderer = new TelegramMarkdownRenderer(writer);
        renderer.Render(document);
        writer.Flush();

        return writer.ToString().Trim();
    }

    // --- ЛОГИКА ОБРАБОТКИ ФОРМУЛ ---

    // Regex ищет 3 типа формул:
    // 1. $$ ... $$ (Блочная)
    // 2. \[ ... \] (Блочная)
    // 3. $ ... $   (Инлайн)
    private static readonly Regex MathRegex = new Regex(
        @"(\$\$[\s\S]+?\$\$)|(\\\[[\s\S]+?\\\])|(\$[^$\n]+?\$)",
        RegexOptions.Compiled);

    private string ProcessMathBlocks(string input)
    {
        return MathRegex.Replace(input, match =>
        {
            string rawMatch = match.Value;
            string content;

            // Определяем тип и извлекаем контент без оберток
            if (rawMatch.StartsWith("$$"))
            {
                content = rawMatch.Substring(2, rawMatch.Length - 4);
            }
            else if (rawMatch.StartsWith("\\["))
            {
                content = rawMatch.Substring(2, rawMatch.Length - 4);
            }
            else // Starts with $
            {
                content = rawMatch.Substring(1, rawMatch.Length - 2);
            }

            // Конвертируем внутренности формулы в Unicode
            // ВАЖНО: Мы удаляем сами знаки $ и $$, чтобы Markdig считал это обычным текстом.
            // (Так как мы уже превратили формулу в красивые Unicode символы)
            return LatexHelper.ConvertToUnicode(content);
        });
    }

    // --- ЛОГИКА МАСКИРОВКИ ССЫЛОК ---

    private static readonly Regex LinkRegex = new Regex(@"(!?\[.*?\])\((.*?)\)", RegexOptions.Compiled);

    private (string MaskedText, Dictionary<string, string> Map) MaskUrls(string input)
    {
        var map = new Dictionary<string, string>();
        int counter = 0;

        var masked = LinkRegex.Replace(input, match =>
        {
            var prefix = match.Groups[1].Value;
            var urlContent = match.Groups[2].Value;
            // Безопасный токен без спецсимволов
            var token = $"XURLTOKENX{counter++}X";
            map[token] = urlContent;
            return $"{prefix}({token})";
        });

        return (masked, map);
    }

    private string RestoreUrls(string input, Dictionary<string, string> map)
    {
        foreach (var kvp in map)
        {
            input = input.Replace(kvp.Key, kvp.Value);
        }
        return input;
    }
}