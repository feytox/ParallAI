using ParallAI.TeleBot.Core.Callback.CallbackArgs;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public record CompareArgs(int Index, bool IsAddingNewConfig) : ICallbackArgs<CompareArgs>
{
    public static CompareArgs Parse(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException($"Invalid compare args: {string.Join(", ", args)}");

        var data = args[0];
        if (data == "+")
            return new CompareArgs(0, true);

        return int.TryParse(data, out var value)
            ? new CompareArgs(value, false)
            : throw new ArgumentException($"Invalid argument for compare callback {data}");
    }
}