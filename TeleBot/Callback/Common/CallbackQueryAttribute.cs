namespace TeleBot.Callback.Common;

public class CallbackQueryAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}