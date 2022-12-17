using UnityEngine;
public abstract class StateMachine : MonoBehaviour
{
    protected State state;
 
    public void SetState(State newState)
    {
        if (state != null) { 
            state.Exit();
	    }
        state = newState; 
        newState.Enter();
    }
}
 
