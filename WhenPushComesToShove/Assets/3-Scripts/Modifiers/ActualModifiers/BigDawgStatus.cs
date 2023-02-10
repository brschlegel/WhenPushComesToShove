using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigDawgStatus : BaseModifier
{
    public override void Init()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponent<Rigidbody2D>().mass *= 2;
            t.localScale *= 2;
            KnockbackData[] kbData = t.GetComponentsInChildren<KnockbackData>();
            foreach(KnockbackData d in kbData)
            {
                d.strength *= 2;
            }
        }
        
        base.Init();
    }

    public override void CleanUp()
    {
        foreach(Transform t in GameState.players)
        {
            t.GetComponent<Rigidbody2D>().mass /= 2;
            t.localScale /= 2;
            KnockbackData[] kbData = t.GetComponentsInChildren<KnockbackData>();
            foreach(KnockbackData d in kbData)
            {
                d.strength /= 2;
            }
        }
        base.CleanUp();
    }
}
