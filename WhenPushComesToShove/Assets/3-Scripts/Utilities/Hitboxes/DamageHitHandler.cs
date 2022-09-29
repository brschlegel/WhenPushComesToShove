using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitHandler : HitHandler
{

    public Health health;
    public bool damageEnabled;
    public override void ReceiveHit(HitEvent e)
    {
        if(damageEnabled)
        {
            AttackData data = e.hitbox.attackData;
            if (data != null)
            {
                health.TakeDamage(data.damage);
            }
        }
    

    }
}
