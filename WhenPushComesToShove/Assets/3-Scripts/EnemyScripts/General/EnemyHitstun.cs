using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyHitstun : Hitstun
{  
    [SerializeField]
    private MovementController movement;

    private void Start()
    {
        sourcesToIgnore = new List<string>() {"Move"};
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
