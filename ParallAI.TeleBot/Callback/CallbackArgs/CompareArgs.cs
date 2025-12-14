using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.TeleBot.Core.Callback.Common;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public class CompareArgs : ICallbackArgs
{
    public int Index { get; private set; }
    public bool IsAddingNewConfig { get; private set; }

    public void Parse(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException($"Invalid compare args: {args}");
        var data = args[0];
        if (data == "+")
            IsAddingNewConfig = true;
        else
            Index = int.Parse(data);
    }
}