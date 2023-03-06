using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundHitHandler : HitHandler
{
    public override void ReceiveHit(HitEvent e)
    {
        string hitName = e.hitbox.gameObject.name;
        Debug.Log("Hit Name: " + hitName);
        if(hitName == "LightHitbox")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.lightShove);
        }
        else if(hitName == "HeavyHitbox")
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.heavyShove);
        }
    }
}
