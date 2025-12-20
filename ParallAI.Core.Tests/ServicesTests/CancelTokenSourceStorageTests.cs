using ParallAI.Core.Services;

namespace ParallAI.Core.Tests.ServicesTests;

[TestFixture]
public class CancelTokenSourceStorageTests
{
    private CancelTokenSourceStorage ctss;

    [SetUp]
    public void Setup()
    {
        ctss = new CancelTokenSourceStorage();
    }

    [Test]
    public void AddSource_CtsIsContained()
    {
        var token = Guid.NewGuid();
        var cts = new CancellationTokenSource();
        ctss.AddSource(token, cts);
        var result = ctss.GetSource(token);
        Assert.That(result, Is.EqualTo(cts));
    }

    [Test]
    public void GetSource_CtsIsNotContained()
    {
        var token = Guid.NewGuid();
        Assert.Catch<KeyNotFoundException>(() => ctss.GetSource(token));
    }

    [Test]
    public void DeleteSource_CtsIsContained()
    {
        var token = Guid.NewGuid();
        var cts = new CancellationTokenSource();
        ctss.AddSource(token, cts);
        ctss.DeleteSource(token);
        Assert.Catch<KeyNotFoundException>(() => ctss.GetSource(token));
    }
    
    [Test]
    public void DeleteSource_CtsIsNotContained()
    {
        var token = Guid.NewGuid();
        Assert.Catch<KeyNotFoundException>(() => ctss.DeleteSource(token));
    }
}