using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitHandler : HitHandler
{

    public Health health;

    private float epsilon = .001f;
    public override void ReceiveHit(HitEvent e)
    {
        if(GameState.damageEnabled)
        {
            AttackData data = e.hitbox.attackData;
            if (data != null)
            {
                if (data.damage > epsilon)
                {
                    health.TakeDamage(data.damage, e.hitbox.owner.name);
                }
            }
        }
    

    }
}
