using System.Text.Json;
using AICore.ValueTypes;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.ValueTypes;
using NUnit.Framework;
using RichardSzalay.MockHttp;

namespace Tests;

[TestFixture]
public class OpenAiGenServiceTests
    : HttpGenServiceTests<OpenAiGenService, OpenAICompatibleProvider, OpenAiRequest, OpenAiResponse>
{
    protected override string ExpectedUrl => "https://api.openai.com/v1/chat/completions";
    protected override OpenAiGenService CreateService(HttpClient client) => new(client);
    protected override OpenAICompatibleProvider CreateProvider() => new(new Uri(ExpectedUrl), ValidApiKey);

    [Test]
    public override async Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse()
    {
        var fakeResponseMessage = new OpenAiMessage("fake response text", OpenAiMessage.MessageRole.Assistant);
        var fakeResponseChoice = new OpenAiResponse.Choice(fakeResponseMessage);
        var fakeApiResponse = new OpenAiResponse([fakeResponseChoice]);

        var expectedAiResponse = fakeApiResponse.ToTextResponse();
        var expectedRequest = OpenAiRequest.Create(ModelId, DefaultPrompt, DefaultSettings);

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .WithHeaders("Authorization", $"Bearer {ValidApiKey}")
            .WithContent(JsonSerializer.Serialize(expectedRequest, JsonSerializerOptions.Web))
            .Respond("application/json", JsonSerializer.Serialize(fakeApiResponse));

        var result = await Service.Generate(DefaultProvider, ModelId, DefaultPrompt, DefaultSettings);

        MockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeEquivalentTo(expectedAiResponse);
    }
}