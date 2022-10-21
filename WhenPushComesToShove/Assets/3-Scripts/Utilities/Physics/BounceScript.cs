using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public delegate void BounceDelegate(Collision2D collision, WallData data);

[RequireComponent(typeof(Collider2D))]
public class BounceScript : MonoBehaviour
{
    public event BounceDelegate onBounce; 
    [SerializeField]
    private VelocitySetter vs;

    //Commented Out TO Work With New System

    //https://www.fabrizioduroni.it/2017/08/25/how-to-calculate-reflection-vector/
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    WallData data = collision.collider.GetComponent<WallData>();
    //
    //    if (data != null)
    //    {
    //        onBounce?.Invoke(collision, data);
    //        //Can't modify the source list during loop
    //        Dictionary<string, Vector2> changes = new Dictionary<string, Vector2>();
    //        Vector2 N = collision.GetContact(0).normal;
    //        foreach (string id in vs.sources.Keys)
    //        {
    //            if (vs.activeTweens.ContainsKey(id))
    //            {
    //                Vector2 vel = vs.sources[id];
    //                Vector2 L = -vel.normalized;
    //                Vector2 R = 2 * (Vector2.Dot(N, L)) * N - L;
    //                changes.Add(id, R);
    //            }
    //         
    //        }
    //
    //        foreach (string id in changes.Keys)
    //        {
    //            Tween tween = vs.activeTweens[id];
    //            tween.fullPosition += (1- data.elasticity);
    //            vs.AddSource(id, changes[id]);
    //        }
    //    }
    //}
}
