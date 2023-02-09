using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rather than learn Unity's statemachine for animations, we decided to roll our own after many frustrating days trying to understand the animator, which seemed more complicated than we needed
//Part of that decision was to better integrate the enemy behaviour with the animations, but enemies were cut :(
//States are each their own scripts, which handle the playing of their animation, and transitioning out of themselves
//The brain handles transitioning into states and serves as a point of contact for all animation needs
public class StateBrain : MonoBehaviour
{
    [SerializeField]
    protected State currentState;
    [SerializeField]
    private bool printInfo;

    protected void ChangeState(State newState)
    {
        if(printInfo)
        {
            Debug.Log("Going from: " + currentState.id + " TO: " + newState.id);
        }

        currentState.enabled = false;
        currentState = newState;
        currentState.enabled = true;
    }

    public State CurrentState
    {
        get{ return currentState;}
    }

}
