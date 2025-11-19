namespace ParallAI.Core.States;

public enum RequestStep
{
    Prompt
}

public class RequestState() : SequentialState<RequestStep>(Enum.GetValues<RequestStep>());