using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHitHandler : HitHandler
{
    public KnockbackReciever kbReciever;
    public override void RecieveHit(HitEvent e)
    {
        AttackData data = e.hitbox.data;
        Vector2 direction = e.hitbox.transform.right;
        kbReciever.TakeKnockback(data.strength, direction.normalized);
    }
}
