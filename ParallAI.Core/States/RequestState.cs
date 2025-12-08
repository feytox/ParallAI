using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class RequestState(RequestConfig config) : UserState
{
    public RequestConfig Config { get; private set; } = config;
}