namespace ParallAI.TeleBot.Core.Callback.CallbackArgs;

public enum SettingsElementAction
{
    Choose,
    Edit,
    Remove,
    None
}

public class SettingsElementArgs : ICallbackArgs
{
    public int Index { get; private set; }
    public SettingsElementAction Action { get; private set; } = SettingsElementAction.None;

    public bool IsAction => Action != SettingsElementAction.None;
    
    public void Parse(string[] args)
    {
        if (args.Length < 1 || args.Length > 2)
            throw new ArgumentException($"Invalid settings element args: {args}");
        
        if (int.TryParse(args[0], out var value))
            Index = value;
        else
            throw new Exception($"Argument {value} for settings element should be integer");

        if (args.Length == 2)
        {
            var action = args[1];
            switch (action)
            {
                case "choose":
                    Action = SettingsElementAction.Choose;
                    break;
                case "edit":
                    Action = SettingsElementAction.Edit;
                    break;
                case "remove":
                    Action = SettingsElementAction.Remove;
                    break;
                default:
                    throw new ArgumentException($"Invalid action for settings element: {action}");
            }
        }
    }
}