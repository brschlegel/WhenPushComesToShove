 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//RProcesses HitEvents for either a hitbox or a hurtbox
//Technically only one per hurtbox/hitbox, but HitEventSplitter can get around that
public abstract class HitHandler : MonoBehaviour 
{
    public abstract void ReceiveHit(HitEvent e);

    public void ProcessHit(HitEvent e)
    {
        if (e.hitbox == null || e.hurtbox == null)
        {
            return;
        }

        foreach(string s in tagsToIgnore)
        {
            if(e.hitbox.CompareTag(s) || e.hurtbox.CompareTag(s))
            {
               return;
            }
        }

        ReceiveHit(e);
    }

    [Tooltip("Will ignore hits from these tags: PUT TAG ON HITBOX OBJECT")]
    public List<string> tagsToIgnore;
}
