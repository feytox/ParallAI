namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public enum SettingsElementAction
{
    Choose,
    Edit,
    Remove,
    None
}

public record SettingsElementArgs(int Index, SettingsElementAction Action) : ICallbackArgs<SettingsElementArgs>
{
    public bool IsAction => Action != SettingsElementAction.None;

    public static SettingsElementArgs Parse(string[] args)
    {
        if (args.Length is < 1 or > 2)
            throw new ArgumentException($"Invalid settings element args: {string.Join(", ", args)}");

        var index = int.TryParse(args[0], out var value)
            ? value
            : throw new Exception($"Argument {value} for settings element should be integer");

        if (args.Length != 2)
            return new SettingsElementArgs(index, SettingsElementAction.None);

        var actionStr = args[1];
        var action = actionStr switch
        {
            "choose" => SettingsElementAction.Choose,
            "edit" => SettingsElementAction.Edit,
            "remove" => SettingsElementAction.Remove,
            _ => throw new ArgumentException($"Invalid action for settings element: {actionStr}")
        };

        return new SettingsElementArgs(index, action);
    }
}