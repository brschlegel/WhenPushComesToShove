using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEvent
{
    public Hitbox hitbox;
    public Hurtbox hurtbox;
    public HitEvent(Hitbox hit, Hurtbox hurt)
    {
        this.hitbox = hit;
        this.hurtbox = hurt;
    }

}

public class HitboxManager : MonoBehaviour
{
    static public HitboxManager instance;
    
    private Dictionary<HitHandler, List<HitEvent>> hitEvents;

    void Awake()
    {
        if(instance == null)
        {
            hitEvents = new Dictionary<HitHandler, List<HitEvent>>();
            instance = this;
        }
    }
    public void RegisterHit(Hitbox hitbox, Hurtbox hurtbox)
    {
        if(!hitEvents.ContainsKey(hurtbox.handler))
        {
            hitEvents.Add(hurtbox.handler, new List<HitEvent>());
        }
        hitEvents[hurtbox.handler].Add(new HitEvent(hitbox, hurtbox));
    }

    private void SendHits()
    {
        if(hitEvents.Count > 0)
        {
            foreach(HitHandler handler in hitEvents.Keys)
            {
                HitEvent currentEvent = null;
                foreach(HitEvent e in hitEvents[handler])
                {
                    //Check ownership
                    if(e.hurtbox.owner != null && e.hurtbox.owner == e.hitbox.owner)
                    {
                        continue;
                    }
                    //Check priority
                    if(currentEvent == null || currentEvent.hurtbox.priority <= e.hurtbox.priority)
                    {
                        currentEvent = e;
                    }
                    //Check hitbox priority
                    if(currentEvent.hurtbox == e.hurtbox && e.hitbox.priority > currentEvent.hitbox.priority)
                    {
                        currentEvent = e;
                    }
                }
                if(currentEvent != null)
                {
                    handler.ProcessHit(currentEvent);
                    if(currentEvent.hitbox.handler != null)
                    {
                        currentEvent.hitbox.handler.ProcessHit(currentEvent);
                    }
                }
            }
        }
    }
    
    public void LateUpdate()
    {
        SendHits();
        hitEvents.Clear();
    }

    //Reset instance when switching scenes
    public void OnDestroy()
    {
        instance = null;
    }
}
