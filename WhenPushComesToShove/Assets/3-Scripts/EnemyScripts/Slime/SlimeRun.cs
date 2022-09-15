using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeRun : State
{
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Chase chase;
    private void OnEnable()
    {
        anim.Play("Base.Slime_Run", 0);
    }

    // Update is called once per frame
    void Update()
    {
        target = PickTarget();
        if (target != null)
        {
            chase.SetTarget(target);
        }
        if(chase.closeEnough)
        {
            enabled = false;
            InvokeOnStateExit(true);
        }
    }

    private void OnDisable()
    {

    }

    private Transform PickTarget()
    {
        float dist = float.PositiveInfinity;
        Transform closest = null;
        foreach(Transform t in GameState.players)
        {
            //Run only if player is alive
            if (!t.GetComponentInChildren<Health>().dead)
            {
                float temp = Vector2.Distance(transform.position, t.position);
                if (temp < dist)
                {
                    dist = temp;
                    closest = t;
                }
            }
        }
        return closest;
    }
}
