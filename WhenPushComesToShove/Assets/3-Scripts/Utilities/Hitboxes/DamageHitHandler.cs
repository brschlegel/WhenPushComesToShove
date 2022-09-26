using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitHandler : HitHandler
{

    public Health health;
    public override void ReceiveHit(HitEvent e)
    {
        AttackData data = e.hitbox.attackData;
        if(data != null)
        {
            health.TakeDamage(data.damage);
        }
    

    }
}
