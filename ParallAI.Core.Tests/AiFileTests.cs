using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests;

[TestFixture]
public class AiFileTests
{
    [Test]
    public void IsImage_MimeTypeIsImage()
    {
        var fileInfo = new AiFileInfo("id", "image");
        var file = new AiFile([], fileInfo);
        var result = file.IsImage;
        Assert.That(result, Is.True);
    }
    
    [Test]
    public void IsImage_MimeTypeIsNotImage()
    {
        var fileInfo = new AiFileInfo("id", "text");
        var file = new AiFile([], fileInfo);
        var result = file.IsImage;
        Assert.That(result, Is.False);
    }
}