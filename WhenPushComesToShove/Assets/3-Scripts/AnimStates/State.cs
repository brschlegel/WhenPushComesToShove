using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void StateReturn(bool success); 
public abstract class State : MonoBehaviour
{
    public event StateReturn onStateExit;
    public string id;
    [HideInInspector]
    public Animator anim;

    protected void InvokeOnStateExit(bool success)
    {
        onStateExit?.Invoke(success);
    }
}
