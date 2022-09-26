using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : State
{
    [HideInInspector]
    public PlayerMovementScript mover;

    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Idle");
    }

    private void Update()
    {
        
        if(mover.IsMoving)
        {
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }
}
