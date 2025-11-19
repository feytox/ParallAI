using ParallAI.Core.States;

namespace TeleBot.Example.States;

public enum RequestStep
{
    Prompt
}

public class RequestState() : SequentialState<RequestStep>(Enum.GetValues<RequestStep>());