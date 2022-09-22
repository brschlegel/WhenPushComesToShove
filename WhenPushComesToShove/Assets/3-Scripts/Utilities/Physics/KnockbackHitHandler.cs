using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackHitHandler : HitHandler
{
    public KnockbackReciever kbReciever;
    public override void ReceiveHit(HitEvent e)
    {
        KnockbackData data = e.hitbox.knockbackData;   
        kbReciever.TakeKnockback(data.strength, data.GetDirection(e));
    }
}
