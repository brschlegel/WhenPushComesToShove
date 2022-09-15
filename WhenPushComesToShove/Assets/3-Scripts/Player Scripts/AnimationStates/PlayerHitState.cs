using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitState : State
{
    [HideInInspector]
    public Hitstun hitstun;

    private void OnEnable()
    {
       
    }

    private void Update()
    {
        if(!hitstun.inHitstun)
        {
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }

    private void OnDisable()
    {

    }
}
