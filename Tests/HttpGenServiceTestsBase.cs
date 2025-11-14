using System.Net;
using System.Text.Json;
using AICore.ValueTypes;
using FluentAssertions;
using Infrastructure.Services;
using Infrastructure.ValueTypes;
using NUnit.Framework;
using RichardSzalay.MockHttp;

#pragma warning disable CS8618

namespace Tests;

[TestFixture]
public abstract class HttpGenServiceTests<TService, TProvider, TRequest, TResponse> 
    where TService : HttpGenService<TProvider, TRequest, TResponse> 
    where TProvider : AiProvider
    where TResponse : IGenResponse
{
    protected const string ModelId = "modelId";
    protected const string ValidApiKey = "TEST_API_KEY";

    protected MockHttpMessageHandler MockHttp;
    protected TService Service;
    private HttpClient httpClient;

    protected abstract string ExpectedUrl { get; }
    protected TProvider DefaultProvider;
    protected Prompt DefaultPrompt;
    protected PromptSettings DefaultSettings;

    [SetUp]
    public void BaseSetUp()
    {
        MockHttp = new MockHttpMessageHandler();
        httpClient = MockHttp.ToHttpClient();
        Service = CreateService(httpClient);
        
        DefaultPrompt = new Prompt("prompt");
        DefaultSettings = PromptSettings.Default;
        DefaultProvider = CreateProvider();
    }
    
    protected abstract TService CreateService(HttpClient client);
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
        
        await Service.Awaiting(s => s.Generate(DefaultProvider, ModelId, DefaultPrompt, DefaultSettings))
            .Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task Generate_WhenApiResponseIsMalformedJson_ThrowsJsonException()
    {
        const string malformedJson = "{ \"invalid_json\": ";

        MockHttp.When(HttpMethod.Post, ExpectedUrl)
            .Respond("application/json", malformedJson);
        
        await Service.Awaiting(s => s.Generate(DefaultProvider, ModelId, DefaultPrompt, DefaultSettings))
            .Should().ThrowAsync<JsonException>();
    }
}