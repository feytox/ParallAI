using Markdig;
using ParallAI.MarkdownV2.Utils;

namespace ParallAI.MarkdownV2;

public class MarkdownV2Converter
{
    private readonly MarkdownPipeline pipeline;

    public MarkdownV2Converter()
    {
        pipeline = new MarkdownPipelineBuilder()
            .UsePipeTables()
            .UseTaskLists()
            .UseAutoLinks()
            .UseEmphasisExtras()
            .UseMathematics()
            .UseSoftlineBreakAsHardlineBreak()
            .Build();
    }

    public string Convert(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
            return string.Empty;

        var (textWithMaskedUrls, urlMap) = UrlProtector.MaskUrls(markdown);
        var textWithUnicodeMath = MathProcessor.ProcessMathBlocks(textWithMaskedUrls);
        var cleanMarkdown = UrlProtector.RestoreUrls(textWithUnicodeMath, urlMap);

        var document = Markdown.Parse(cleanMarkdown, pipeline);

        using var writer = new StringWriter();
        var renderer = new TelegramMarkdownRenderer(writer);
        renderer.Render(document);
        writer.Flush();

        return writer.ToString().Trim();
    }
}
