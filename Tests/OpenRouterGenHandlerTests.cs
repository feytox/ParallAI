using System.Text.Json;
using AICore.Entities;
using AICore.ValueTypes;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.ValueTypes;
using RichardSzalay.MockHttp;

namespace Tests;

public class OpenRouterGenHandlerTests
    : HttpGenHandlerTests<OpenRouterGenHandler, OpenRouterProvider, OpenRouterRequest, OpenAiResponse>
{
    protected override string ExpectedUrl => "https://openrouter.ai/api/v1/chat/completions";

    protected override OpenRouterGenHandler CreateHandler(HttpClient client, AiModel model, OpenRouterProvider provider)
    {
        return new OpenRouterGenHandler(provider, model, client, FakeFileService);
    }

    protected override OpenRouterProvider CreateProvider() => new(ValidApiKey);

    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var fakeResponseMessage =
            new OpenAiMessage(OpenAiContent.Create("fake response text"), OpenAiMessage.MessageRole.Assistant);
        var fakeResponseChoice = new OpenAiResponse.Choice(fakeResponseMessage);
        var fakeApiResponse = new OpenAiResponse([fakeResponseChoice]);

        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = OpenRouterRequest.Create(ModelId, DefaultPrompt, DefaultSettings);

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("Authorization", $"Bearer {ValidApiKey}")
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));

        var result = await Handler.Generate(DefaultPrompt, DefaultSettings);

        MockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}