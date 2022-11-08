using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlyHitOnceHitHandler : HitHandler
{
    public override void ReceiveHit(HitEvent e)
    {
        e.hitbox.OwnersToIgnore.Add(e.hurtbox.owner);
    }
}
