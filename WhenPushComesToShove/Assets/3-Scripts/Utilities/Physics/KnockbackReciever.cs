using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    [Range(0,1)]
    private float knockBackResistance;
    [SerializeField]
    private VelocitySetter vs;
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, KnockbackType type = KnockbackType.Normal)
    {
        
        float kb = force * (1-knockBackResistance);
        string id = vs.GenerateUniqueID();
        Tween t = DOVirtual.Float(kb, 0, 1.0f, v => vs.UpdateVelocityMagnitude(id, v));
        vs.AddSourceTween(id, direction * kb, t );
    }

    private void Start()
    {
        //If we don't have a velocity setter, try to find one 
        if(vs == null)
        {
            vs = GetComponent<VelocitySetter>();
        }
    }
}
