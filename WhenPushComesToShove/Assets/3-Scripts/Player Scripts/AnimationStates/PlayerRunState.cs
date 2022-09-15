using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : State
{
    [HideInInspector]
    public VelocitySetter vs;
    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Run");
    }

    private void Update()
    {
        if(vs.QuerySource("playerMovement", out Vector2 vel))
        {
            if(vel.magnitude <= .01f)
            {
                this.enabled = false;
                InvokeOnStateExit(true);
            }
        }
    }
}
