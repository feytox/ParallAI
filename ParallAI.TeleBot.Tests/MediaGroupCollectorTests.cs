using ParallAI.TeleBot.Services;
using Message = Telegram.Bot.Types.Message;

namespace ParallAI.TeleBot.Tests;

[TestFixture]
public class MediaGroupCollectorTests
{
    [Test]
    public async Task CollectMessages_WithoutMediaGroup()
    {
        var mediaGroupCollector = new MediaGroupCollector();
        var testMessages = new Message { Text = "test message" };
        var result = await mediaGroupCollector.CollectMessages(testMessages);
        var expected = new[] { testMessages };
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public Task CollectMessages_WithMediaGroup()
    {
        var mediaGroupCollector = new MediaGroupCollector();
        var firstMsg = new Message { MediaGroupId = "test ID" };
        var secondMsg = new Message { MediaGroupId = "test ID" };
        _ = Task.Run(async () =>
        {
            var result = await mediaGroupCollector.CollectMessages(firstMsg);
            await mediaGroupCollector.CollectMessages(secondMsg);
            var expected = new[] { firstMsg, secondMsg };
            Assert.That(result, Is.EquivalentTo(expected));
        }, CancellationToken.None);
        return Task.CompletedTask;
    }

    [Test]
    public Task CollectMessages_IsNotFirstMessage()
    {
        var mediaGroupCollector = new MediaGroupCollector();
        var firstMsg = new Message { MediaGroupId = "test ID" };
        var secondMsg = new Message { MediaGroupId = "test ID" };
        _ = Task.Run(async () =>
        {
            await mediaGroupCollector.CollectMessages(firstMsg);
            var result = await mediaGroupCollector.CollectMessages(secondMsg);
            Assert.That(result, Is.Null);
        }, CancellationToken.None);
        return Task.CompletedTask;
    }

    [Test]
    public Task CollectMessages_MultipleMediaGroups()
    {
        var mediaGroupCollector = new MediaGroupCollector();

        var firstMsgFirstGroup = new Message { MediaGroupId = "ID 1" };
        var secondMsgFirstGroup = new Message { MediaGroupId = "ID 1" };

        var firstMsgSecondGroup = new Message { MediaGroupId = "ID 2" };
        var secondMsgSecondGroup = new Message { MediaGroupId = "ID 2" };

        _ = Task.Run(async () =>
        {
            var firstGroup = await mediaGroupCollector.CollectMessages(firstMsgFirstGroup);
            var secondGroup = await mediaGroupCollector.CollectMessages(firstMsgSecondGroup);

            await mediaGroupCollector.CollectMessages(secondMsgFirstGroup);
            await mediaGroupCollector.CollectMessages(secondMsgSecondGroup);

            var expectedFirstGroup = new[] { firstMsgFirstGroup, secondMsgFirstGroup };
            var expectedSecondGroup = new[] { firstMsgSecondGroup, secondMsgSecondGroup };

            Assert.That(firstGroup, Is.EquivalentTo(expectedFirstGroup));
            Assert.That(secondGroup, Is.EquivalentTo(expectedSecondGroup));
        }, CancellationToken.None);
        return Task.CompletedTask;
    }
}