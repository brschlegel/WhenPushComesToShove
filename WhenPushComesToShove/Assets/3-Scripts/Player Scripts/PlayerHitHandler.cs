using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitHandler : HitHandler
{
  [SerializeField]
  private KnockbackReciever kbReciever;
  public override void RecieveHit(HitEvent e)
  {
    KnockbackData data = e.hitbox.knockbackData;
    Vector2 direction =  e.hurtbox.transform.position-e.hitbox.transform.position;
    kbReciever.TakeKnockback(data.strength, direction.normalized);
  }
}
