using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHitstun : Hitstun
{   
    [SerializeField]
    private PlayerMovementScript mover;
    private bool stunned = false;

    protected override void Stun()
    {
        if (!stunned)
        {
            mover.ForceLockMovement();
            stunned = true;
        }
    }

    protected override void Unstun()
    {
        if (stunned)
        {
            mover.ForceUnlockMovement();
            stunned = false;
        }
    }


}
