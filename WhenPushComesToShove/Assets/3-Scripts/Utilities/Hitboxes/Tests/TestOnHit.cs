using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestOnHit : HitHandler
{
   public override void ReceiveHit(HitEvent e)
   {
        Debug.Log(e.hurtbox.gameObject.name + " was hit by " + e.hitbox.gameObject.name);
   }
}
