using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : State
{
    [HideInInspector]
    public PlayerMovementScript mover;
    void OnEnable()
    {
        anim.Play("Base Layer.AN_Player_Run");
    }

    private void Update()
    {
        
        if(!mover.IsMoving)
        {
            this.enabled = false;
            InvokeOnStateExit(true);
        }
    }
}
