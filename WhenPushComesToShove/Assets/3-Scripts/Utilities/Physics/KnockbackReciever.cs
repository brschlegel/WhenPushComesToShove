using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

   
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, KnockbackType type = KnockbackType.Normal)
    {
        float kb = force / rb.mass ;
        Debug.Log(direction * kb);
        rb.AddForce(direction * kb);
    }

  
}
