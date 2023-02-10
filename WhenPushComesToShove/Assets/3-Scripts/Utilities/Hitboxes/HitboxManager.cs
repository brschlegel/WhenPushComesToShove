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

//Singleton to handle all hitbox hurtbox overlaps
//Banks every hit event, then at end of frame send events out based on ownership and priority
//Hurtbox must have a hithandler to process a hit event, a hitbox optionally can have one
public class HitboxManager : MonoBehaviour
{
    static public HitboxManager instance;
    
    private Dictionary<GameObject, List<HitEvent>> hitEvents;

    void Awake()
    {
        if(instance == null)
        {
            hitEvents = new Dictionary<GameObject, List<HitEvent>>();
            instance = this;
        }
    }
    public void RegisterHit(Hitbox hitbox, Hurtbox hurtbox)
    {
        if(!hitEvents.ContainsKey(hurtbox.owner))
        {
            hitEvents.Add(hurtbox.owner, new List<HitEvent>());
        }
        hitEvents[hurtbox.owner].Add(new HitEvent(hitbox, hurtbox));
    }

    private void SendHits()
    {
        if(hitEvents.Count > 0)
        {
            foreach(GameObject owner in hitEvents.Keys)
            {
                HitEvent currentEvent = null;
                foreach(HitEvent e in hitEvents[owner])
                {
                    //If hurtbox and hitbox have same owner, do not send event
                    if(e.hurtbox.owner != null && e.hurtbox.owner == e.hitbox.owner)
                    {
                        continue;
                    }
                    //If the hitbox or hurtbox ignores the other's owner, do not send event
                    if(e.hitbox.OwnersToIgnore.Contains(e.hurtbox.owner) || e.hurtbox.OwnersToIgnore.Contains(e.hitbox.owner))
                    {
                        continue;
                    }
                    //Check hurtbox priority
                    if(currentEvent == null || currentEvent.hurtbox.priority < e.hurtbox.priority)
                    {
                        currentEvent = e;
                    }

                    //Check hitbox priority, only relevant if current best event and event being checked have same hurtbox
                    if(currentEvent.hurtbox == e.hurtbox && e.hitbox.priority > currentEvent.hitbox.priority)
                    {
                        currentEvent = e;
                    }
                }
                //If we have an event to send...
                if(currentEvent != null)
                {
                    //Send it
                    currentEvent.hurtbox.handler.ProcessHit(currentEvent);
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
