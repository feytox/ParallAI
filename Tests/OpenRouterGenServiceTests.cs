using System.Text.Json;
using AICore.ValueTypes;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.ValueTypes;
using RichardSzalay.MockHttp;

namespace Tests;

public class OpenRouterGenServiceTests : HttpGenServiceTests<OpenRouterGenService, OpenRouterProvider, OpenRouterRequest, OpenAiResponse>
{
    protected override string ExpectedUrl => "https://openrouter.ai/api/v1/chat/completions";
    protected override OpenRouterGenService CreateService(HttpClient client) => new(client);
    protected override OpenRouterProvider CreateProvider() => new(ValidApiKey);

    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var fakeResponseMessage = new OpenAiMessage("fake response text", OpenAiMessage.MessageRole.Assistant);
        var fakeResponseChoice = new OpenAiResponse.Choice(fakeResponseMessage);
        var fakeApiResponse = new OpenAiResponse([fakeResponseChoice]);
        
        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = OpenRouterRequest.Create(ModelId, defaultPrompt, defaultSettings);
        
        mockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("Authorization", $"Bearer {ValidApiKey}")
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));
        
        var result = await service.Generate(defaultProvider, ModelId, defaultPrompt, defaultSettings);
        
        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}