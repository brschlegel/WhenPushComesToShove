using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBrain : MonoBehaviour
{
    [SerializeField]
    protected State currentState;
    public Transform target;

    protected void ChangeState(State newState)
    {
        currentState.enabled = false;
        currentState = newState;
        currentState.enabled = true;
    }

}
