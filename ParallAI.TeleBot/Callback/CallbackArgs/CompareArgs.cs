using ParallAI.TeleBot.Core.Callback.CallbackArgs;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public class CompareArgs : ICallbackArgs
{
    public int Index { get; private set; }
    public bool IsAddingNewConfig { get; private set; }

    public void Parse(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException($"Invalid compare args: {args}");
        var data = args[0];
        if (data == "+")
            IsAddingNewConfig = true;
        else if (int.TryParse(data, out var value))
            Index = value;
        else
            throw new ArgumentException($"Invalid argument for compare callback {data}");
    }
}