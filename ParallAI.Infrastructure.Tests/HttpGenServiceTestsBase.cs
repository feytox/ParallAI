using System.Net;
using System.Text.Json;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using ParallAI.Core;
using ParallAI.Core.Entities;
using ParallAI.Core.ValueTypes;
using ParallAI.Infrastructure.Services;
using ParallAI.Infrastructure.ValueTypes;
using RichardSzalay.MockHttp;

#pragma warning disable CS8618

namespace ParallAI.Infrastructure.Tests;

[TestFixture]
public abstract class HttpGenHandlerTests<THandler, TProvider, TRequest, TResponse> 
    where THandler : HttpGenHandler<TProvider, TRequest, TResponse> 
    where TProvider : AiProvider
    where TResponse : IGenResponse
{
    protected const string ModelId = "modelId";
    protected const string ValidApiKey = "TEST_API_KEY";
    private const string ModelName = "Model Name";

    protected MockHttpMessageHandler MockHttp;
    protected THandler Handler;
    protected IFileService FakeFileService;
    private HttpClient httpClient;

    protected abstract string ExpectedUrl { get; }
    protected TextPrompt DefaultPrompt;
    protected PromptSettings DefaultSettings;

    [SetUp]
    public virtual void BaseSetUp()
    {
        MockHttp = new MockHttpMessageHandler();
        httpClient = MockHttp.ToHttpClient();
        FakeFileService = A.Fake<IFileService>();

        var provider = CreateProvider();
        var model = new AiModel(Guid.NewGuid(), ModelId, ModelName, provider);
        Handler = CreateHandler(httpClient, model, provider);
        
        DefaultPrompt = new TextPrompt("prompt");
        DefaultSettings = PromptSettings.Default;
    }
    
    protected abstract THandler CreateHandler(HttpClient client, AiModel model, TProvider provider);
    
    protected abstract TProvider CreateProvider();
    
    public abstract Task Generate_WhenApiCallIsSuccessful_FormsRequestCorrectlyAndReturnsResponse();
    
    [TestCase(HttpStatusCode.Unauthorized)]
    [TestCase(HttpStatusCode.Forbidden)]
    [TestCase(HttpStatusCode.BadRequest)]
    [TestCase(HttpStatusCode.InternalServerError)]
    public async Task Generate_WhenApiReturnsError_ThrowsHttpRequestException(HttpStatusCode statusCode)
    {
        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .Respond(statusCode);
        
        await Handler.Awaiting(s => s.Generate(DefaultPrompt, DefaultSettings))
            .Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task Generate_WhenApiResponseIsMalformedJson_ThrowsJsonException()
    {
        const string malformedJson = "{ \"invalid_json\": ";

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .Respond("application/json", malformedJson);
        
        await Handler.Awaiting(s => s.Generate(DefaultPrompt, DefaultSettings))
            .Should().ThrowAsync<JsonException>();
    }
}