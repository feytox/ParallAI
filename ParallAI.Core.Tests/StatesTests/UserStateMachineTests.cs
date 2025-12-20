using FakeItEasy;
using ParallAI.Core.States;
using ParallAI.Core.States.Common;

namespace ParallAI.Core.Tests.StatesTests;

[TestFixture]
public class UserStateMachineTests
{
    private UserStateMachine userStateMachine;
    private UserState state;

    [SetUp]
    public void Setup()
    {
        userStateMachine = new UserStateMachine();
        state = A.Fake<UserState>();
    }
    
    [Test]
    public void Push_SingleState_CurrentStateIsPushedState()
    {
        userStateMachine.Push(state);
        Assert.That(userStateMachine.Current, Is.EqualTo(state));
    }

    [Test]
    public void Pop_PositiveStatesCount_CurrentStateIsMainMenu()
    {
        userStateMachine.Push(state);
        userStateMachine.Pop();
        Assert.That(userStateMachine.Current, Is.TypeOf(typeof(MainMenuState)));
    }

    [Test]
    public void Pop_ZeroStatesCount_CatchException()
    {
        Assert.Catch<InvalidOperationException>(() => userStateMachine.Pop());
    }

    [Test]
    public void TryPop_PositiveStatesCount_ReturnTrue()
    {
        userStateMachine.Push(state);
        var result = userStateMachine.TryPop();
        Assert.That(result);
    }

    [Test]
    public void TryPop_ZeroStatesCount_ReturnFalse()
    {
        var result = userStateMachine.TryPop();
        Assert.That(result, Is.False);
    }

    [Test]
    public void TryPop_AcceptPrevState_MustHaveHappened()
    {
        var prevState = A.Fake<UserState>(options => options.Implements<IPrevStateHandler>());
        
        userStateMachine.Push(prevState);
        userStateMachine.Push(state);
        var current = userStateMachine.Current;
        userStateMachine.TryPop();
        A.CallTo(() => ((IPrevStateHandler)prevState).AcceptPrevState(current)).MustHaveHappened();
    }
    
    [Test]
    public void TryPop_AcceptPrevState_MustNotHaveHappened()
    {
        var prevState = A.Fake<UserState>(options => options.Implements<IPrevStateHandler>());
        
        userStateMachine.Push(prevState);
        userStateMachine.Push(state);
        var current = userStateMachine.Current;
        userStateMachine.TryPop(reactivate: false);
        A.CallTo(() => ((IPrevStateHandler)prevState).AcceptPrevState(current)).MustNotHaveHappened();
    }

    [Test]
    public void TryPop_IsCancelled_AcceptPrevStateWithNull_MustHaveHappened()
    {
        var prevState = A.Fake<UserState>(options => options.Implements<IPrevStateHandler>());
        
        userStateMachine.Push(prevState);
        userStateMachine.Push(state);
        userStateMachine.TryPop(cancelled: true);
        A.CallTo(() => ((IPrevStateHandler)prevState).AcceptPrevState(null)).MustHaveHappened();
    }
}