using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBrain : MonoBehaviour
{
    [SerializeField]
    protected State currentState;
    public Transform target;
    [SerializeField]
    private bool printInfo;

    protected void ChangeState(State newState)
    {
        if(printInfo)
            Debug.Log("Going from: " + currentState.id + " TO: " + newState.id);
        currentState.enabled = false;
        currentState = newState;
        currentState.enabled = true;
    }

}
