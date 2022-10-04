using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    private ProjectileMode pMode;
    [SerializeField]
    private Transform exclamationTransform;
    [SerializeField]
    private ParticleSystem exclamationPS;
    [SerializeField]
    private Transform gustTransform;
    [SerializeField]
    private ParticleSystem gustPS;


   
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, GameObject instigator)
    {
        pMode.enabled = true;
        pMode.AddForce(direction * force);
        //Tell the projectile mode hitbox to ignore whoever sent it into projectile mode
        pMode.pHitbox.OwnersToIgnore.Add(instigator);

        //VFX
        exclamationTransform.right = direction;
        exclamationPS.Play();

        gustTransform.right = direction;

        
        float xSign = Mathf.Sign(direction.x);
        Vector2 compareTo = -xSign * Vector2.right;
        gustPS.transform.localScale = new Vector3(Mathf.Abs(gustPS.transform.localScale.x) * xSign,gustPS.transform.localScale.y, gustPS.transform.localScale.z ); 
        //gustPS.GetComponent<ParticleSystemRenderer>().flip = xSign == -1 ? new Vector3(1,0,0) : new Vector3(0,0,0);
        
        float angle = Vector2.Angle(compareTo, -direction);

        if(angle >= 90)
        {
            gustTransform.right = direction;
        }
        else
        {
            gustTransform.right = Vector2.right;
        }
        gustPS.Play();
  
    }

  
}
