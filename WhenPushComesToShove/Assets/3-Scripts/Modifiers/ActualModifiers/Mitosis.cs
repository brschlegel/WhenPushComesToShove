using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Freya;

public class Mitosis : EventModifier
{

    [SerializeField]
    private float splitForce;
    [SerializeField]
    private float splitAngle;
    public override void Init()
    {
        key = "Ball_Hit";
        base.Init();
    }

    protected override void OnEvent(MessageArgs args)
    {
        //Gotta wait so we dont mess with foreach loops
       CoroutineManager.StartGlobalCoroutine(SetUpEndOfFrame(args));

    }

    private void SetUpObject(GameObject g, HitEvent e, Vector2 dir, float force )
    {
        g.transform.localScale *= .5f;
        
        ProjectileMode pMode = g.GetComponentInChildren<ProjectileMode>();
        pMode.enabled = true;
        pMode.AddForce(dir * force);
        pMode.Mass *= .5f; 
        
        //Could break, but if we are sending messages from a hurtbox that doesn't have a splitter we have other problems
        List<HitHandler> handlers = ((HitEventSplitter)g.GetComponentInChildren<Hurtbox>().handler).outputs;
        
        //Products of mitosis should not mitosis
        //Really all of this is trying to future proof, making sure we don't remove the wrong thing
        HitHandler toRemove = null;
        foreach(HitHandler h in handlers)
        {
            if(h is MessageOnHit)
            {
                MessageOnHit m = (MessageOnHit)h;
                if(m.keyStart == "Ball")
                {
                    toRemove = h;
                }
            }
        }

        if(toRemove != null)
        {
            handlers.Remove(toRemove);
        }

    }

    private IEnumerator SetUpEndOfFrame(MessageArgs args)
    {
        yield return new WaitForEndOfFrame();
         HitEvent e = (HitEvent)args.objectArg;
        Transform parent = e.hurtbox.owner.transform.parent;
        //not actually left and right, just a handy signifier of one vs the other
        GameObject left = Instantiate(e.hurtbox.owner, e.hurtbox.owner.transform.position, Quaternion.identity, parent);
        GameObject right  = Instantiate(e.hurtbox.owner, e.hurtbox.owner.transform.position, Quaternion.identity, parent);
        Vector2 dir;
        float force = splitForce;
        if(e.hitbox.knockbackData != null)
        {
            dir = e.hitbox.knockbackData.GetDirection(e);
            force += e.hitbox.knockbackData.strength; 
        }
        else
        {
            dir = (e.hurtbox.transform.position - e.hitbox.transform.position).normalized;
        }

        float angle = splitAngle * Mathf.Deg2Rad;
        
        SetUpObject(left, e, dir.RotateAround(e.hitbox.Center,angle ), force);
        SetUpObject(right, e, dir.RotateAround(e.hitbox.Center, -angle ), force);
        Destroy(e.hurtbox.owner);
    }

}
