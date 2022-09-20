using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHitstun : Hitstun
{   
    [SerializeField]
    private PlayerInputHandler inputHandler;
    private bool stunned = false;
    private void Start()
    {
        sourcesToIgnore = new List<string>(){"playerMovement", "playerDash"};
    }
    protected override void Stun()
    {
       inputHandler.ForceLockMovement();
       stunned = true;
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
