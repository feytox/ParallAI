using FluentAssertions;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.Tests;

[TestFixture]
public class AiMessageTests
{
    [TestCase("another text")]
    [TestCase("")]
    public void TextMessages_AppendTextOneTime(string addedText)
    {
        var textMessage = new TextMessage("text");
        var result = textMessage.AppendText(addedText);
        Assert.That(((TextMessage)result).Text, Is.EqualTo($"text\n{addedText}"));
    }
    
    [TestCase("another text", 10)]
    [TestCase("", 10)]
    public void TextMessages_AppendTextSeveralTime(string addedText, int count)
    {
        var textMessage = new TextMessage("text");
        var result = textMessage.AppendText(addedText);
        Console.WriteLine(result);
        for (int i = 0; i < count - 1; i++)
        {
            var newTextMessage = new TextMessage(((TextMessage)result).Text);
            result = newTextMessage.AppendText(addedText);
        }

        Assert.That(((TextMessage)result).Text, Is.EqualTo(
            $"text" + string.Concat(Enumerable.Range(0, count).Select(i => $"\n{addedText}"))));
    }

    [TestCase("text", Role.User)]
    [TestCase("", Role.Assistant)]
    public void FileMessage_Create_NotNullText(string text, Role role)
    {
        var files = new AiFileInfo[] { };
        var result = FileMessage.Create(files,text, role);
        var expected = new FileMessage(text, files, role);
        result.Should().BeEquivalentTo(expected);
    }
    
    [TestCase(null, Role.User)]
    [TestCase(null, Role.Assistant)]
    public void FileMessage_Create_NullText(string? text, Role role)
    {
        var files = new AiFileInfo[] { };
        var result = FileMessage.Create(files,text, role);
        Assert.That(result.Text, !Is.EqualTo(text));
        Assert.Multiple(() =>
        {
            Assert.That(result.Files, Is.EqualTo(files));
            Assert.That(result.Role, Is.EqualTo(role));
        });
    }
}