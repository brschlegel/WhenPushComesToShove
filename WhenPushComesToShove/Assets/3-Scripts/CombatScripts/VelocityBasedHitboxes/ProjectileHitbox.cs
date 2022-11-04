using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AttackData))]
public class ProjectileHitbox : MonoBehaviour
{
    private AttackData data;
    [HideInInspector]
    public ProjectileMode pMode;
    private Hitbox hitbox;
    [SerializeField]
    private float instanceDamageMultiplier = 1;
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
    }

    // Update is called once per frame
    void Update()
    {

        data.damage = instanceDamageMultiplier * Mathf.Clamp(pMode.Mass * pMode.Velocity.magnitude * GlobalSettings.velocityDamageCoeff , 0, GlobalSettings.velocityDamageCap);
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
}
