using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MovementController
{
    public bool closeEnough;
    [SerializeField]
    private Transform chaseTarget;

    private void Update()
    {
        if(chaseTarget == null)
            return;
        move.target = chaseTarget.position;
        if(Vector2.Distance(transform.position, move.target) > destinationTolerance)
        {
            //If the target has just gotten far enough away
            if(closeEnough)
            {
                startMovingEvent.Invoke();
            }
            closeEnough = false;
        }
        else
        {
            if(!closeEnough)
            {
                endMovingEvent.Invoke();
            }
            move.enabled = false;
            closeEnough = true;
        }
        if(!closeEnough)
        {
            move.enabled = true;
        }
    }

    public void SetTarget(Transform target)
    {
        chaseTarget = target;
        move.enabled = true;
    }

}
