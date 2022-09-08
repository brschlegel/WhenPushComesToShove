using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHitHandler : HitHandler
{
    public KnockbackReciever kbReciever;
    public override void ReceiveHit(HitEvent e)
    {
        KnockbackData data = e.hitbox.knockbackData;
        Vector2 direction = e.hitbox.transform.right;
        kbReciever.TakeKnockback(data.strength, data.GetDirection(e));
    }
}
