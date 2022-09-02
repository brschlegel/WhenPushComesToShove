using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public struct TweenInfo{
    public Ease ease;
    public float speed;
    public TweenInfo(Ease ease, float speed)
    {
        this.ease = ease;
        this.speed = speed;
    }
}
public class KnockbackReciever : MonoBehaviour
{
    [SerializeField]
    private VelocitySetter vs;

    public float mass;
    public Ease ease;
    public float speed;
    /// <summary>
    /// Applies Knockback
    /// </summary>
    /// <param name="force">Magnitude of vector</param>
    /// <param name="direction">Direction of vector</param>
    public void TakeKnockback(float force, Vector2 direction, KnockbackType type = KnockbackType.Normal)
    {
        
        float kb = force ;
        string id = vs.GenerateUniqueID();
        Tween t = DOVirtual.Float(kb, 0, speed, v => vs.UpdateVelocityMagnitude(id, v)).SetSpeedBased().SetEase(ease);
 
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
