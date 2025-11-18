namespace TeleBot.Callback.Common;

[AttributeUsage(AttributeTargets.Class)]
public class CallbackQueryAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}