using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalSettings
{
   //Physics
   public readonly static float frictionCoeff = .6f;

   //Wall Bounce Damage
   public readonly static float wallDamageCoeff = 1;
   public readonly static float wallDamageCap = 20.0f;

   //Velocity Based Damage;
   public readonly static float velocityHitboxDelay = .2f;
   public readonly static float velocityDamageCoeff = .2f;
   public readonly static float velocityDamageCap = 10.0f;
   public readonly static float velocityKnockbackCoeff = 2;
   public readonly static float velocityKnockbackCap = 70.0f;
}
