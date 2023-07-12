public class StateMachine
{
    public State currentState { get; private set; }
    public bool isInitialized = false;

    public void Init(State startingState)
    {
        isInitialized = true;
        currentState = startingState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
