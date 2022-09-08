using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    private VelocitySetter vs;

    private float mass;
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, KnockbackType type = KnockbackType.Normal)
    {
        float kb = force / mass ;
        float speed = mass * GlobalSettings.frictionCoeff;
        string id = vs.GenerateUniqueID();
        Tween t = DOVirtual.Float(kb, 0, speed, v => vs.UpdateVelocityMagnitude(id, v)).SetSpeedBased().SetEase(Ease.InOutExpo);
 
        vs.AddSourceTween(id, direction * kb, t );
    }

    private void Start()
    {
        //If we don't have a velocity setter, try to find one 
        if(vs == null)
        {
            vs = GetComponent<VelocitySetter>();

        }
        mass = vs.GetComponent<Rigidbody2D>().mass;

    }
}
