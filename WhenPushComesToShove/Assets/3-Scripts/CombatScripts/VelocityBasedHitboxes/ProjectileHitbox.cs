using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(AttackData))]
public class ProjectileHitbox : MonoBehaviour
{
    private AttackData data;
    [HideInInspector]
    public ProjectileMode pMode;
    private Hitbox hitbox;
    [SerializeField]
    private float instanceDamageMultiplier = 1;

    private List<Tuple<float, float>> velocityDamageThresholds;
    // Start is called before the first frame update
    void OnEnable()
    {
        if(data == null)
        {
            Init();
        }
    }

    public void Init()
    {
        data = GetComponent<AttackData>();
        hitbox = GetComponent<Hitbox>();
        velocityDamageThresholds = new List<Tuple<float, float>>();
        velocityDamageThresholds.Add(new Tuple<float, float>(4, 7));
        velocityDamageThresholds.Add(new Tuple<float, float>(18,15));
        velocityDamageThresholds.Add(new Tuple<float, float>(25,25));
    }

    // Update is called once per frame
    void Update()
    {
        data.damage = instanceDamageMultiplier * Mathf.Clamp(pMode.Mass * GetThresholdValue(pMode.Velocity.magnitude) * GlobalSettings.velocityDamageCoeff , 0, GlobalSettings.velocityDamageCap);
    }

    public List<GameObject> OwnersToIgnore
    {
        get
        {
            if(hitbox == null)
            {
                Init();
            }
            return hitbox.OwnersToIgnore;
    
        }
    }

    private float GetThresholdValue(float velocity)
    {
        float thresholdValue = 0;
        for(int i = 0; i < velocityDamageThresholds.Count; i++)
        {
            if(pMode.Velocity.magnitude <= velocityDamageThresholds[i].Item1)
            {
                return thresholdValue;
            }
            thresholdValue = velocityDamageThresholds[i].Item2;
        }
        return thresholdValue;
    }
}
