using ParallAI.TeleBot.Core.Callback.CallbackArgs;
using ParallAI.Core.ValueTypes;

namespace ParallAI.TeleBot.Callback.CallbackArgs;

public class AskArgs : ICallbackArgs
{
    public RequestMode RequestMode { get; private set; }

    public void Parse(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException($"Invalid ask args: {args}");
        var requestMode = args[0];
        switch (requestMode)
        {
            case "single":
                RequestMode = RequestMode.Single;
                break;
            case "continuous":
                RequestMode = RequestMode.Continuous;
                break;
            case "settings":
                RequestMode = RequestMode.Settings;
                break;
            default:
                throw new ArgumentException($"Invalid request mode for ask callback: {requestMode}");
        }
    }
}