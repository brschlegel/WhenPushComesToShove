using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    [HideInInspector]
    public VelocitySetter vs;

    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Idle");
    }

    private void Update()
    {
        // if (vs.QuerySource("playerMovement", out Vector2 vel))
        // {
        //     if (vel.magnitude >= .01f)
        //     {
        //         this.enabled = false;
        //         InvokeOnStateExit(true);
        //     }
        // }
    }
}
