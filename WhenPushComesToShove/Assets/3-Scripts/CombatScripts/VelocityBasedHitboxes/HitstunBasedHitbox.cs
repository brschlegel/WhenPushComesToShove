using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Hitstun))]
public class HitstunBasedHitbox : VelocityBasedHitbox
{
    private Hitstun hitstun;
    
    protected override void Init()
    {
        base.Init();
        hitstun = GetComponent<Hitstun>();
    
    }

    private void Update()
    {
        UpdateData();
        if(hitstun.inHitstun)
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
}
