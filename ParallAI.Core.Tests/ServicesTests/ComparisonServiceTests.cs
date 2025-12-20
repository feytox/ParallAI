using FakeItEasy;
using ParallAI.Core.Entities;
using ParallAI.Core.Services;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests.ServicesTests;

[TestFixture]
public class ComparisonServiceTests
{
    private ComparisonService comparisonService;
    private IGenerationService genService;
    private readonly AiMessage prompt = new TextMessage("Prompt");
    private Preset preset;
    private AiModel model;
    
    [SetUp]
    public void Setup()
    {
        genService = A.Fake<IGenerationService>();
        comparisonService = new ComparisonService(genService);
        preset = new Preset(Guid.NewGuid(), "name", PromptSettings.Default);
        model = new AiModel(Guid.NewGuid(), "id", "name", A.Fake<AiProvider>());
    }

    [Test]
    public async Task Generate_TwoElements_ReturnCompareResult()
    {
        var responses = new AiResponse[]{ new("Response 1"), new("Response 2") };
        var orchestratorResponse = new AiResponse("Comparison");
        A.CallTo(() => 
                genService.Generate(A<AiModel>._, A<AiMessage[]>._, A<PromptSettings>._, CancellationToken.None))
                .ReturnsNextFromSequence(
                    Task.FromResult(responses[0]), Task.FromResult(responses[1]), Task.FromResult(orchestratorResponse));
        
        var compElem1 = new CompareElement(preset, model);
        var compElem2 = new CompareElement(preset, model);
        var orchestrator = new CompareElement(preset, model);
        var config = new CompareConfig([compElem1, compElem2], orchestrator, DateTime.Now);
        
        var result = await comparisonService.Generate(prompt, config, CancellationToken.None);
        Assert.That(result.Responses, Is.EquivalentTo(responses));
        Assert.That(result.OrchestratorResponse, Is.EqualTo(orchestratorResponse));
    }

    [Test]
    public async Task Generate_CancellationTokenGivenToGenService()
    {
        var cts = new CancellationTokenSource();
        var config = A.Fake<CompareConfig>();
        
        await comparisonService.Generate(prompt, config, cts.Token);

        A.CallTo(() => 
            genService.Generate(A<AiModel>._, A<AiMessage[]>._, A<PromptSettings>._, cts.Token))
            .MustHaveHappened();
    }
}