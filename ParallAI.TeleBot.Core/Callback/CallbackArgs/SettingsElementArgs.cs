namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;


public class SettingsElementArgs : ICallbackArgs
{
    public int Index { get; private set; }
    public string? Action { get; private set; }

    public bool IsAction => !string.IsNullOrEmpty(Action);
    
    public void Parse(string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
            throw new ArgumentException($"Invalid settings element args: {args}");

        Index = int.Parse(args[0]);

        if (args.Length == 2)
            Action = args[1];
    }
}