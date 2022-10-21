using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisStun : EnemyStun
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
