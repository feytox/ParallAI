namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public record SettingsStateArgs(string Content) : ICallbackArgs<SettingsStateArgs>
{
    public static SettingsStateArgs Parse(string[] args)
    {
        return args.Length == 1
            ? new SettingsStateArgs(args[0])
            : throw new ArgumentException($"Invalid settings state args: {string.Join(", ", args)}");
    }
}