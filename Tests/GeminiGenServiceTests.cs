#region

using System.Text.Json;
using AICore.ValueTypes;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.ValueTypes;
using NUnit.Framework;
using RichardSzalay.MockHttp;

#endregion


namespace Tests;

[TestFixture]
public class GeminiGenServiceTests : HttpGenServiceTests<GeminiGenService, GeminiProvider, GeminiRequest, GeminiResponse>
{
    protected override string ExpectedUrl => $"https://generativelanguage.googleapis.com/v1beta/models/{ModelId}:generateContent";
    protected override GeminiGenService CreateService(HttpClient client) => new(httpClient);
    protected override GeminiProvider CreateProvider() => new(ValidApiKey);

    [Test]
    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var responsePart = new GeminiContent.Part("fake response text");
        var responseContent = new GeminiContent([responsePart], GeminiContent.MessageRole.Model);
        var responseCandidate = new GeminiResponse.Candidate(responseContent);
        var fakeApiResponse = new GeminiResponse([responseCandidate]);
        
        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = GeminiRequest.Create(defaultPrompt, defaultSettings);

        mockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("x-goog-api-key", ValidApiKey)
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));
        
        var result = await service.Generate(defaultProvider, ModelId, defaultPrompt, defaultSettings);
        
        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}