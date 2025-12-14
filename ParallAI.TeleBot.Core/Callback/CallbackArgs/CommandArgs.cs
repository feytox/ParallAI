using ParallAI.TeleBot.Core.Callback.Common;

namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public class CommandArgs : ICallbackArgs
{
    public string CommandName { get; private set; } = string.Empty;
    
    public void Parse(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException($"Invalid command args: {args}");
        CommandName = args[0];
    }
}