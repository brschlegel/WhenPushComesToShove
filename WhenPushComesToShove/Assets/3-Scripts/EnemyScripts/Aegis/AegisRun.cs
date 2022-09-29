using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisRun : State
{
    [HideInInspector]
    public Transform target;
    [HideInInspector]
    public Chase chase;
    void OnEnable()
    {
        anim.Play("Base Layer.Enemy Run");
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null || target.GetComponentInChildren<PlayerHealth>().dead)
        {
            target = GameState.GetNearestPlayer(transform);
        }

        if(target != null)
        {
            chase.SetTarget(target);
        }
        if(chase.closeEnough)
        {
            enabled = false;
            InvokeOnStateExit(true);
        }
    }
}
