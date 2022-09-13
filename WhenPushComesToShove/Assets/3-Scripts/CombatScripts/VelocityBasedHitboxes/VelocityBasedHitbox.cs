using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityBasedHitbox : MonoBehaviour
{
    [SerializeField]
    private float velocityThreshold;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private GameObject hitboxObject;

    private AttackData attack;
    private KnockbackData kb;

    void Start()
    {
        if(attack == null)
        {
            Init();
        }
    }

    protected virtual void Init()
    {
        attack = hitboxObject.GetComponent<AttackData>();
        kb = hitboxObject.GetComponent<KnockbackData>();
    }

    // Update is called once per frame
    void Update()
    {

        UpdateData();
        if(vs.Velocity.magnitude >= velocityThreshold)
        {
            if(!hitboxObject.activeSelf)
            {
                Activate(true);
            }
        }
        else
        {
            if(hitboxObject.activeSelf)
            {
                Activate(false);
            }
        }
    }

    protected virtual void UpdateData()
    {
        kb.strength = Mathf.Clamp(vs.Velocity.magnitude * vs.Mass * GlobalSettings.velocityKnockbackCoeff, 0, GlobalSettings.velocityKnockbackCap);
        attack.damage = Mathf.Clamp(vs.Velocity.magnitude * vs.Mass * GlobalSettings.velocityDamageCap, 0, GlobalSettings.velocityDamageCap);
    }

    protected virtual void Activate(bool on)
    {
        hitboxObject.SetActive(on);
    }
}
