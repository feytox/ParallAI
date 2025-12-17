namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public interface ICallbackArgs<TSelf> where TSelf : ICallbackArgs<TSelf>
{
    static abstract TSelf Parse(string[] args);
}