using ParallAI.Core.States.Common;
using ParallAI.Core.ValueTypes;

namespace ParallAI.Core.States;

public class CompareState(CompareConfig config) : UserState
{
    public CompareConfig Config { get; private set; } = config;
}