using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisHit : EnemyHit
{
    [HideInInspector]
    public AegisWall wall;
     protected override void OnEnable()
    {
        base.OnEnable();
        wall.stunned = true;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        wall.stunned = false;
    }
}
