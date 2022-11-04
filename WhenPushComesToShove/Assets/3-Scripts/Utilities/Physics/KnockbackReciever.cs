using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    private ProjectileMode pMode;
    [SerializeField]
    private KnockbackVFX vfx;



   
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, GameObject instigator)
    {
        pMode.sourceObject = instigator;
        pMode.enabled = true;
        //Tell the projectile mode hitbox to ignore whoever sent it into projectile mode
        pMode.pHitbox.OwnersToIgnore.Add(instigator);
        pMode.AddForce(direction * force);


        //VFX
        vfx.Play(force, direction);
  
    }

  
}
