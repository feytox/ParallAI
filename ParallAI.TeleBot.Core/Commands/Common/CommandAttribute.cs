namespace ParallAI.TeleBot.Core.Commands.Common;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute(string name, string description) : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public bool HighPriority { get; set; } = false;
}