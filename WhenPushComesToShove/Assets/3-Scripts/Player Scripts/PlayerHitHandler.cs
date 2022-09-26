using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//DEPRECIATED
//TODO: DELTE
public class PlayerHitHandler : HitHandler
{
  [SerializeField]
  private KnockbackReciever kbReciever;
  [SerializeField]
  private Health health;
  public override void ReceiveHit(HitEvent e)
  {
      KnockbackData kbData = e.hitbox.knockbackData;
      AttackData attackData = e.hitbox.attackData;
      Vector2 direction = e.hurtbox.transform.position - e.hitbox.transform.position;
      //kbReciever.TakeKnockback(kbData.strength, direction.normalized);
      health.TakeDamage(attackData.damage);
  }
}
