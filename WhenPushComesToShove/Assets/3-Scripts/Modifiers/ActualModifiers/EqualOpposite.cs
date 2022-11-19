using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EqualOpposite : BaseModifier
{
    private uint eventID;
    public override void Init()
    {
        eventID = Messenger.RegisterEvent("Shove_Hit", OnHit);
    }

    //Called from the MessageOnHit script
    private void OnHit(MessageArgs args)
    {
        HitEvent e = (HitEvent)args.objectArg;
        if(e != null)
        {
            KnockbackData data = e.hitbox.knockbackData;
            KnockbackReciever kbReciever = e.hitbox.owner.GetComponentInChildren<KnockbackReciever>();
            kbReciever.TakeKnockback(data.strength, -data.GetDirection(e), e.hitbox.owner);
        }
    }

    public override void CleanUp()
    {
        Messenger.UnregisterEvent("Shove_Hit", eventID);
    }
}
