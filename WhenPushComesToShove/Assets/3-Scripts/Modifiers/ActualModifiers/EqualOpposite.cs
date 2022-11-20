using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualOpposite : EventModifier
{
  
    public override void Init()
    {
        key = "Shove_Hit";
        base.Init();
    }

    //Called from the MessageOnHit script
    protected override void OnEvent(MessageArgs args)
    {
        HitEvent e = (HitEvent)args.objectArg;
        if(e != null)
        {
            KnockbackData data = e.hitbox.knockbackData;
            KnockbackReciever kbReciever = e.hitbox.owner.GetComponentInChildren<KnockbackReciever>();
            kbReciever.TakeKnockback(data.strength, -data.GetDirection(e), e.hitbox.owner);
        }
    }

  
}
