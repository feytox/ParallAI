using Markdig.Extensions.TaskLists;
using Markdig.Renderers;

namespace ParallAI.MarkdownV2.Renderers;

public class TelegramTaskListRenderer : MarkdownObjectRenderer<TelegramMarkdownRenderer, TaskList>
{
    protected override void Write(TelegramMarkdownRenderer renderer, TaskList obj)
    {
        var symbol = obj.Checked ? renderer.Options.TaskCompleted : renderer.Options.TaskUncompleted;
        renderer.Write(symbol + " ");
    }
}
