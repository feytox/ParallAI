using System.Text.Json;
using FluentAssertions;
using NUnit.Framework;
using ParallAI.Core.Entities;
using ParallAI.Core.Providers;
using ParallAI.Infrastructure.Services;
using ParallAI.Infrastructure.ValueTypes;
using RichardSzalay.MockHttp;

namespace ParallAI.Infrastructure.Tests;

[TestFixture]
public class OpenAiGenHandlerTests
    : HttpGenHandlerTests<OpenAiGenHandler, OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>
{
    protected override string ExpectedUrl => "https://api.openai.com/v1/chat/completions";

    protected override OpenAiGenHandler CreateHandler(HttpClient client, AiModel model,
        OpenAICompatibleProvider provider)
    {
        return new OpenAiGenHandler(provider, model, client, FakeFileService);
    }

    protected override OpenAICompatibleProvider CreateProvider() => new(new Uri(ExpectedUrl), ValidApiKey);

    [Test]
    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var fakeResponseMessage =
            new OpenAiMessage(OpenAiContent.Create("fake response text"), OpenAiMessage.MessageRole.Assistant);
        var fakeResponseChoice = new OpenAiResponse.Choice(fakeResponseMessage);
        var fakeApiResponse = new OpenAiResponse([fakeResponseChoice]);

        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = OpenAiRequest.Create(ModelId, DefaultPrompt, DefaultSettings);

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("Authorization", $"Bearer {ValidApiKey}")
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));

        var result = await Handler.Generate(DefaultPrompt, DefaultSettings);

        MockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}