using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Services;

public record CompareResult(AiResponse[] Responses, AiResponse OrchestratorResponse);

public class ComparisonService(IGenerationService genService)
{
    public async Task<CompareResult> Generate(AiMessage prompt, CompareConfig config, CancellationToken cancellationToken)
    {
        var request = new[] { prompt };
        var responses = await Task.WhenAll(config.Elements
            .Select(async element =>
                await genService.Generate(element.Model, request, element.Preset.PromptSettings, cancellationToken)));

        var orchestrator = config.Orchestrator;
        var orchestratorMessage = CombineResponses(prompt, responses);
        var response = await genService.Generate(orchestrator.Model, [orchestratorMessage],
            orchestrator.Preset.PromptSettings, cancellationToken);

        return new CompareResult(responses, response);
    }

    private static AiMessage CombineResponses(AiMessage initialPrompt, AiResponse[] responses)
    {
        var formattedResponses = responses
            .Select(response => response.Text)
            .Select((text, i) => $"<response_{i + 1}>\n{text}\n</response_{i + 1}>");

        var responsesText = $"\n\n---\n\n{string.Join("\n\n", formattedResponses)}";
        return initialPrompt.AppendText(responsesText);
    }
}