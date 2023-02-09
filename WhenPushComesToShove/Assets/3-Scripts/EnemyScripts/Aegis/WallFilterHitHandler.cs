using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class WallFilterHitHandler : HitHandler
{
    [SerializeField]
    private Hurtbox fallbackHurtbox;

    public UnityAction onBlock;

    public override void ReceiveHit(HitEvent e)
    {
        //If the hitbox is closer to the wall than the fallback
        if(Vector2.Distance(e.hitbox.Center, e.hurtbox.Center) <= Vector2.Distance(e.hitbox.Center, fallbackHurtbox.Center))
        {
            //Block the attack!
            Debug.Log("Blocked");
            if (e.hitbox.tag == "Shove")
            {
                onBlock?.Invoke();
            }
        }
        else
        {
            fallbackHurtbox.handler.ProcessHit(e);
        }
        
    }
}
