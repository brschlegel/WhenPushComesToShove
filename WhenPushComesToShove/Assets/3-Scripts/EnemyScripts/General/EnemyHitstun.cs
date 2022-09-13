using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class EnemyHitstun : Hitstun
{  
    private MovementController movement;

    private void Start()
    {
        sourcesToIgnore = new List<string>() {"Move"};
        movement = GetComponent<MovementController>();
    }

    protected override void Stun()
    {
        movement.LockMovement();
    }

    protected override void Unstun()
    {
        movement.UnlockMovement();
    }
}
