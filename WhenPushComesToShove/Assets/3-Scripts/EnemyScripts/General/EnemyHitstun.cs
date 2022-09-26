using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHitstun : Hitstun
{  
    [SerializeField]
    private MovementController movement;

    protected override void Stun()
    {
        movement.LockMovement();
    }

    protected override void Unstun()
    {
        movement.UnlockMovement();
    }
}
