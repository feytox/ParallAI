namespace ParallAI.TeleBot;

[AttributeUsage(AttributeTargets.Class)]
public class MainMenuAttribute(string name, MenuOrder weight) : Attribute
{
    public string NameUI { get; } = name;
    
    public int Weight { get; } = (int)weight;
}

public enum MenuOrder
{
    Start = 1,
    Help = 2, 
    AddPreset = 3
}