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
    protected GameObject hitboxObject;

    private AttackData attack;
    private KnockbackData kb;
    private IEnumerator enumerator;

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
        if(vs.ListedVelocity.magnitude >= velocityThreshold)
        {
            if(!hitboxObject.activeSelf && enumerator == null)
            {
                Activate(true);
            }
        }
        else
        {
            if(hitboxObject.activeSelf && enumerator != null)
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
        if (on)
        {
            enumerator = CoroutineManager.StartGlobalCoroutine(WaitToActivate());
        }
        else
        {
            if (enumerator != null)
            {
                CoroutineManager.StopGlobalCoroutine(enumerator);
                enumerator = null;
            }
            hitboxObject.SetActive(false);
        }
   
    }

    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(GlobalSettings.velocityHitboxDelay);
        if(hitboxObject != null)
            hitboxObject.SetActive(true);
    }
}
