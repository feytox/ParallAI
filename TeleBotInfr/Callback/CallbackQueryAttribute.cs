namespace TeleBotInfr.Callback;

[AttributeUsage(AttributeTargets.Class)]
public class CallbackQueryAttribute(string key) : Attribute
{
    public string Key { get; } = key;
}