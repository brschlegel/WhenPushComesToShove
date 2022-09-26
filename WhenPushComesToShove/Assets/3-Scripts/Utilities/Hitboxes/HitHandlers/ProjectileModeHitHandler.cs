using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileModeHitHandler : HitHandler
{
    [SerializeField]
    ProjectileMode myPMode;
    public override void ReceiveHit(HitEvent e)
    {
        //If hitbox is a projectile mode hitbox
        if(e.hitbox.TryGetComponent(out ProjectileHitbox pHitbox))
        {
            //If the projectile mode is enabled (Should be if this hitbox is active anyway, but doesn't hurt to check)
            if(pHitbox.pMode.enabled)
            {
                //Enable my projectile mode
                myPMode.enabled = true;
            }
        }
    }
}
