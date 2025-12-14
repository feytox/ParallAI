using ParallAI.TeleBot.Core.Callback.CallbackArgs;

namespace ParallAI.TeleBot.Core.Callback.Common;

public class CallbackData
{
    public string Key { get; }
    public string[] Args { get; }

    public CallbackData(string data)
    {
        var splitData = data.Split(':');
        Key = splitData[0];
        Args = splitData.Skip(1).ToArray();
    }
}

public class CallbackData<TArgs> where TArgs : ICallbackArgs, new()
{
    public string Key { get; }
    public TArgs Args { get; }
    
    public CallbackData(CallbackData source)
    {
        Key = source.Key;
        Args = new TArgs();
        Args.Parse(source.Args);
    }
}