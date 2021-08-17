using UnityEngine;

public class SimpleFsm : MonoBehaviour
{
    public enum eState
    {
        Move = 0,
        Attack
    }

    private eState currentState = eState.Move;
    private int stateCount = 0;
    private const int CHANGE_TIME = 5;

    void Update()
    {
        switch (currentState)
        {
            case eState.Move:  updateMoveState(); break;
            case eState.Attack:  updateAttackState(); break;
        }
    }

    private void updateMoveState()
    {
        ++stateCount;
        Debug.Log("Update Move " + stateCount);
        if (CHANGE_TIME < stateCount)
        {
            changeState(eState.Attack);
        }
    }

    private void updateAttackState()
    {
        ++stateCount;
        Debug.Log("Update Attack " + stateCount);
        if (CHANGE_TIME < stateCount)
        {
            changeState(eState.Move);
        }
    }

    private void enterState(eState state)
    {
        stateCount = 0;

        switch (state)
        {
            case eState.Move: Debug.Log("Enter Move"); break;
            case eState.Attack: Debug.Log("Enter Attack"); break;
        }
    }

    private void exitState(eState state)
    {
        switch (state)
        {
            case eState.Move: Debug.Log("Exit Move"); break;
            case eState.Attack: Debug.Log("Exit Attack"); break;
        }
    }

    private void changeState(eState state)
    {
        eState prevState = currentState;
        currentState = state;
        exitState(prevState);
        enterState(currentState);
    }
}
