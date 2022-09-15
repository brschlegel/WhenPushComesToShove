using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHitstun : Hitstun
{   
    [SerializeField]
    private PlayerInputHandler inputHandler;
    private void Start()
    {
        sourcesToIgnore = new List<string>(){"playerMovement", "playerDash"};
    }
    protected override void Stun()
    {
       inputHandler.ForceLockMovement();
    }

    protected override void Unstun()
    {
        inputHandler.ForceUnlockMovement();
    }


}
