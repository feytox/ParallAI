using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Callback.Common;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public class AskArgs : ICallbackArgs
{
    public string RequestMode { get; private set; } = string.Empty;

    public void Parse(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException($"Invalid ask args: {args}");
        RequestMode = args[0];
    }
}