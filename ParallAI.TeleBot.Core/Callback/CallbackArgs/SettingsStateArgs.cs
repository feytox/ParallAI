namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public class SettingsStateArgs : ICallbackArgs
{
    public string Content { get; private set; } = string.Empty;
    
    public void Parse(string[] args)
    {
        if (args.Length != 1) throw new ArgumentException($"Invalid settings state args: {args}");
        Content = args[0];
    }
}