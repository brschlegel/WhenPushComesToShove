using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHitHandler : HitHandler
{

    public Health health;

    public override void ReceiveHit(HitEvent e)
    {
        if(GameState.damageEnabled)
        {
            Debug.Log("in ");
            AttackData data = e.hitbox.attackData;
            if (data != null)
            {
                if (data.damage > 0)
                {
                    Debug.Log("damge: " + data.damage);
                    health.TakeDamage(data.damage, e.hitbox.owner.name);
                }
            }
        }
    

    }
}
