using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerHitstun : Hitstun
{   
    private PlayerInputHandler inputHandler;
    private void Start()
    {
        inputHandler = GetComponent<PlayerInputHandler>();
        sourcesToIgnore = new List<string>(){"playerMovement", "playerDash"};
    }
    protected override void Stun()
    {
       inputHandler.ForceLockMovement();
    }

    protected override void Unstun()
    {
        inputHandler.ForceLockMovement();
    }


}
