#region

using System.Net;
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
public abstract class HttpGenServiceTests<TService, TProvider, TRequest, TResponse> 
    where TService : HttpGenService<TProvider, TRequest, TResponse> 
    where TProvider : AiProvider
    where TResponse : IGenResponse
{
    protected const string ModelId = "modelId";
    protected const string ValidApiKey = "TEST_API_KEY";

    protected MockHttpMessageHandler mockHttp;
    protected HttpClient httpClient;
    protected TService service;
    
    protected abstract string ExpectedUrl { get; }
    protected TProvider defaultProvider;
    protected Prompt defaultPrompt;
    protected PromptSettings defaultSettings;

    [SetUp]
    public void BaseSetUp()
    {
        mockHttp = new MockHttpMessageHandler();
        httpClient = mockHttp.ToHttpClient();
        service = CreateService(httpClient);
        
        defaultPrompt = new Prompt("prompt");
        defaultSettings = PromptSettings.Default;
        defaultProvider = CreateProvider();
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
        mockHttp.When(HttpMethod.Post, ExpectedUrl)
            .Respond(statusCode);
        
        await service.Awaiting(s => s.Generate(defaultProvider, ModelId, defaultPrompt, defaultSettings))
            .Should().ThrowAsync<HttpRequestException>();
    }
    
    [Test]
    public async Task Generate_WhenApiResponseIsMalformedJson_ThrowsJsonException()
    {
        const string malformedJson = "{ \"invalid_json\": ";

        mockHttp.When(HttpMethod.Post, ExpectedUrl)
            .Respond("application/json", malformedJson);
        
        await service.Awaiting(s => s.Generate(defaultProvider, ModelId, defaultPrompt, defaultSettings))
            .Should().ThrowAsync<JsonException>();
    }
}