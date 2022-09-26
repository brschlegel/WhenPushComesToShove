using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : State
{
    [HideInInspector]
    public ProjectileMode pMode;

    private void OnEnable()
    {
       
    }

    private void Update()
    {
        if(!pMode.enabled)
        {
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }

    private void OnDisable()
    {

    }
}
