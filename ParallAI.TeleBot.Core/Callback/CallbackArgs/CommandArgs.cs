namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public record CommandArgs(string CommandName) : ICallbackArgs<CommandArgs>
{
    public static CommandArgs Parse(string[] args)
    {
        return args.Length == 1
            ? new CommandArgs(args[0])
            : throw new ArgumentException($"Invalid command args: {string.Join(", ", args)}");
    }
}