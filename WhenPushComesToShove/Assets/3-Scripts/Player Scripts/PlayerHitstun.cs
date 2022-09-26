using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHitstun : Hitstun
{   
    [SerializeField]
    private PlayerInputHandler inputHandler;
    private bool stunned = false;

    protected override void Stun()
    {
        if (!stunned)
        {
            inputHandler.ForceLockMovement();
            stunned = true;
        }
    }

    protected override void Unstun()
    {
        if (stunned)
        {
            inputHandler.ForceUnlockMovement();
            stunned = false;
        }
    }


}
