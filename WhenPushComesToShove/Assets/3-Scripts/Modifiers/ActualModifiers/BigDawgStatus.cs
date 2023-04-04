using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDawgStatus : BaseModifier
{

    private float modValue = 1.5f;
    public override void Init()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponent<Rigidbody2D>().mass *= modValue;
            t.localScale *= modValue;
            KnockbackData[] kbData = t.GetComponentsInChildren<KnockbackData>();
            foreach(KnockbackData d in kbData)
            {
                d.strength *= modValue;
            }
            PlayerHeavyShoveScript heavy = t.GetComponentInChildren<PlayerHeavyShoveScript>();
            heavy.forceMultiplier *= modValue;
            PlayerDashScript dash = t.GetComponentInChildren<PlayerDashScript>();
            dash.dashSpeed *= modValue;
            PlayerHealth health = t.GetComponentInChildren<PlayerHealth>();
            health.maxHealth *= modValue;
        }
        
        base.Init();
    }

    public override void CleanUp()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponent<Rigidbody2D>().mass /= modValue;
            t.localScale /= modValue;
            KnockbackData[] kbData = t.GetComponentsInChildren<KnockbackData>();
            foreach(KnockbackData d in kbData)
            {
                d.strength /= modValue;
            }
            PlayerHeavyShoveScript heavy = t.GetComponentInChildren<PlayerHeavyShoveScript>();
            heavy.forceMultiplier /= modValue;
            PlayerHealth health = t.GetComponentInChildren<PlayerHealth>();
            health.maxHealth /= modValue;
            PlayerDashScript dash = t.GetComponentInChildren<PlayerDashScript>();
            dash.dashSpeed *= modValue;
        }
        base.CleanUp();
    }
}
