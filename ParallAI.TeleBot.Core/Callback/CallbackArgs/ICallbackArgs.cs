namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public interface ICallbackArgs<out TSelf> where TSelf : ICallbackArgs<TSelf>
{
    static abstract TSelf Parse(string[] args);
}