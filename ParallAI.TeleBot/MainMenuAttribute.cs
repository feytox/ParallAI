namespace ParallAI.TeleBot;

[AttributeUsage(AttributeTargets.Class)]
public class MainMenuAttribute(string name, MenuOrder weight) : Attribute //вынести
{
    public string NameUI { get; } = name;
    
    public int Weight { get; } = (int)weight;
}

public enum MenuOrder // вынести 
{
    Start = 1,
    Help = 2, 
    AddPreset = 3
}