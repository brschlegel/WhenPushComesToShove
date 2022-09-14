 using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Right now we are passing around the entire event as its likely we will want data from both the hitbox and the hurtbox when determining what to do with the data in the handler. 
//If we decide this is too bloated to pass around we can look at slimming it down
public abstract class HitHandler : MonoBehaviour 
{
    public abstract void ReceiveHit(HitEvent e);

    public void ProcessHit(HitEvent e)
    {
        foreach(string s in tagsToIgnore)
        {
            if(e.hitbox.CompareTag(s))
            {
               return;
            }
        }

        ReceiveHit(e);
    }

    [Tooltip("Will ignore hits from these tags: PUT TAG ON HITBOX OBJECT")]
    public List<string> tagsToIgnore;
}
