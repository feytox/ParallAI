namespace ParallAI.TeleBot.Commands.UI;

[AttributeUsage(AttributeTargets.Class)]
public class MainMenuAttribute(string name, MenuOrder weight) : Attribute
{
    public string NameUI { get; } = name;
    
    public int Weight { get; } = (int)weight;
}