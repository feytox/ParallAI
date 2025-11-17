using AICore.Entities;
using AICore.Services;
using AICore.ValueTypes;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

#pragma warning disable CS8618

namespace Tests;

[TestFixture]
public class GenerationServiceTests
{
    private IGenService fakeDuckDuckGoService;
    private IGenService fakeGigaChatService;
    private GenerationService generationService;
    private AiResponse fakeGigaChatResponse;
    private AiResponse fakeDuckDuckGoResponse;

    private readonly Prompt prompt = new TextPrompt("Кто ты на самом деле?");
    private readonly PromptSettings settings = PromptSettings.Default;

    [SetUp]
    public void SetUp()
    {
        fakeDuckDuckGoService = A.Fake<IGenService>();
        A.CallTo(() => fakeDuckDuckGoService.ProviderType)
            .Returns(typeof(DuckDuckGoProvider));

        fakeGigaChatService = A.Fake<IGenService>();
        A.CallTo(() => fakeGigaChatService.ProviderType)
            .Returns(typeof(GigaChatProvider));

        fakeDuckDuckGoResponse = new AiResponse("Ответ ДакДакича: Я на самом деле Юра");
        A.CallTo(() => fakeDuckDuckGoService.Generate(A<AiProvider>._, A<AiModel>._, A<Prompt>._, A<PromptSettings>._))
            .Returns(Task.FromResult(fakeDuckDuckGoResponse));

        fakeGigaChatResponse = new AiResponse("Ответ ГигаЧата: Я на самом деле Павел Васильев");
        A.CallTo(() => fakeGigaChatService.Generate(A<AiProvider>._, A<AiModel>._, A<Prompt>._, A<PromptSettings>._))
            .Returns(Task.FromResult(fakeGigaChatResponse));

        generationService = new GenerationService(
            [fakeDuckDuckGoService, fakeGigaChatService]
        );
    }

    [Test]
    public async Task Generate_WhenGigaChatProviderIsPassed_ShouldCallGigaChatServiceAndReturnItsResponse()
    {
        var gigaChatProvider = new GigaChatProvider();
        var model = new AiModel(Guid.NewGuid(), "giga-chat-model", "Test GigaChat", gigaChatProvider);

        var actualResponse = await generationService.Generate(model, prompt, settings);

        actualResponse.Should().Be(fakeGigaChatResponse);

        A.CallTo(() => fakeDuckDuckGoService.Generate(A<AiProvider>._, A<AiModel>._, A<Prompt>._, A<PromptSettings>._))
            .MustNotHaveHappened();
        A.CallTo(() => fakeGigaChatService.Generate(A<AiProvider>._, A<AiModel>._, A<Prompt>._, A<PromptSettings>._))
            .MustHaveHappenedOnceExactly();
    }

    [Test]
    public async Task Generate_WhenProviderIsUnknown_ShouldThrowArgumentException()
    {
        var unknownProvider = new UnknownProvider();
        var modelWithUnknownProvider = new AiModel(Guid.NewGuid(), "unknown-model", "Unknown", unknownProvider);

        await generationService.Awaiting(s => s.Generate(modelWithUnknownProvider, prompt, settings))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{typeof(UnknownProvider)}*");
    }

    [Test]
    public async Task Generate_WhenNoServicesAreProvided_ShouldThrowArgumentException()
    {
        var serviceWithNoProviders = new GenerationService([]);

        var provider = new GigaChatProvider();
        var model = new AiModel(Guid.NewGuid(), "any-model", "Any Model", provider);


        await serviceWithNoProviders.Awaiting(s => s.Generate(model, prompt, settings))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage($"*{typeof(GigaChatProvider)}*");
    }

    private record DuckDuckGoProvider : AiProvider;

    private record GigaChatProvider : AiProvider;

    private record UnknownProvider : AiProvider;
}