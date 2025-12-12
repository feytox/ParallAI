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
public class GeminiGenHandlerTests
    : HttpGenHandlerTests<GeminiGenHandler, GeminiProvider, GeminiRequest, GeminiContent, GeminiResponse>
{
    protected override string ExpectedUrl =>
        $"https://generativelanguage.googleapis.com/v1beta/models/{ModelId}:generateContent";
    protected override string DefaultErrorContent =>
        """{"error":{"code":400,"message":"API key not valid. Please pass a valid API key.","status":"INVALID_ARGUMENT"}}""";
    
    protected override GeminiGenHandler CreateHandler(HttpClient client, AiModel model, GeminiProvider provider)
    {
        return new GeminiGenHandler(provider, model, client, FakeFileService);
    }

    protected override GeminiProvider CreateProvider() => new(ValidApiKey);

    [Test]
    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var responsePart = new GeminiContent.Part("fake response text");
        var responseContent = new GeminiContent([responsePart], GeminiContent.MessageRole.Model);
        var responseCandidate = new GeminiResponse.Candidate(responseContent);
        var fakeApiResponse = new GeminiResponse([responseCandidate]);

        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = GeminiRequest.Create([DefaultMessage.ToGeminiContent()], DefaultSettings);

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("x-goog-api-key", ValidApiKey)
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));

        var result = await Handler.Generate([DefaultMessage], DefaultSettings);

        MockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}