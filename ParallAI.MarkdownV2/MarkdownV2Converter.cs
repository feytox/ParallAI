using Markdig;
using ParallAI.MarkdownV2.Utils;

namespace ParallAI.MarkdownV2;

public class MarkdownV2Converter(MarkdownOptions? options = null)
{
    private readonly MarkdownPipeline pipeline = new MarkdownPipelineBuilder()
        .UsePipeTables()
        .UseTaskLists()
        .UseAutoLinks()
        .UseEmphasisExtras()
        .UseMathematics()
        .UseSoftlineBreakAsHardlineBreak()
        .Build();

    public string Convert(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        var (textWithMaskedUrls, urlMap) = UrlProtector.MaskUrls(markdown);
        var textWithUnicodeMath = MathProcessor.ProcessMathBlocks(textWithMaskedUrls);
        var cleanMarkdown = UrlProtector.RestoreUrls(textWithUnicodeMath, urlMap);

        var document = Markdown.Parse(cleanMarkdown, pipeline);

        using var writer = new StringWriter();
        var renderer = new TelegramMarkdownRenderer(writer, options);
        renderer.Render(document);
        writer.Flush();

        return writer.ToString().Trim();
    }
}
