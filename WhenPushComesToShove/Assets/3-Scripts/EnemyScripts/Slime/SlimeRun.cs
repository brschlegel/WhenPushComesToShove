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
        return GameState.GetNearestPlayer(transform);
    }
}
