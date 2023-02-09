using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void OnHitWithTag(string s);
public class TagFilterHitHandler : HitHandler
{
    public List<string> tagsToFilter;
    public event OnHitWithTag onHitWithTag;
    public HitHandler fallbackHandler;
    
    public override void ReceiveHit(HitEvent e)
    {
        foreach (string s in tagsToFilter)
        {
            if (e.hitbox.CompareTag(s) || e.hurtbox.CompareTag(s))
            {
                onHitWithTag.Invoke(s);
                return;
            }
        }
        fallbackHandler.ProcessHit(e);

    }
}
