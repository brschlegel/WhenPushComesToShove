using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BounceScript))]
public class DamageOnBounce : MonoBehaviour
{
    [SerializeField]
    private Health health;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private List<string> sourcesToIgnore;
    

    // Start is called before the first frame update
    void Start()
    {
        //GetComponent<BounceScript>().onBounce += OnBounce;
    }

    //Commented Out to Work With New System

    //private void OnBounce(Collision2D collision, WallData data)
    //{
    //    float magnitude = 0;
    //    foreach(string source in vs.sources.Keys)
    //    {
    //        if(vs.QuerySource(source, out Vector2 vel) && !sourcesToIgnore.Contains(source))
    //        {
    //            magnitude += vel.magnitude;
    //        }
    //    }
    //    float damage = Mathf.Clamp(magnitude * data.damageMultiplier * GlobalSettings.wallDamageCoeff, float.NegativeInfinity, GlobalSettings.wallDamageCap);
    //    health.TakeDamage(damage, data.gameObject.name);
    //}

   
}
