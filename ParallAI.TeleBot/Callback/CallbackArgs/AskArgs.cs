using ParallAI.Core.ValueTypes;
using ParallAI.TeleBot.Core.Callback.CallbackArgs;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public record AskArgs(RequestMode RequestMode) : ICallbackArgs<AskArgs>
{
    public static AskArgs Parse(string[] args)
    {
        if (args.Length != 1)
            throw new ArgumentException($"Invalid ask args: {string.Join(", ", args)}");

        var requestModeStr = args[0];
        var requestMode = requestModeStr switch
        {
            "single" => RequestMode.Single,
            "continuous" => RequestMode.Continuous,
            "settings" => RequestMode.Settings,
            _ => throw new ArgumentException($"Invalid request mode for ask callback: {requestModeStr}")
        };
        return new AskArgs(requestMode);
    }
}