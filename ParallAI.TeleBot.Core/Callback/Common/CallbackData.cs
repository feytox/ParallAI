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

public class CallbackData<TArgs>(CallbackData source) where TArgs : ICallbackArgs<TArgs>
{
    public string Key { get; } = source.Key;
    public TArgs Args { get; } = TArgs.Parse(source.Args);
}