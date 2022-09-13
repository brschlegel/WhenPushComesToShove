using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hitstun))]
public class HitstunBasedHitbox : VelocityBasedHitbox
{
    private Hitstun hitstun;
    
    protected override void Init()
    {
        base.Init();
        hitstun = GetComponent<Hitstun>();
    }
}
