using TeleBot.Services;

namespace TeleBot.Commands;

[AttributeUsage(AttributeTargets.Class)]
public class CommandAttribute(string name, string description, CommandType type = CommandType.Single)
    : Attribute
{
    public string Name { get; } = name;
    public string Description { get; } = description;
    public CommandType Type { get; } = type;
}