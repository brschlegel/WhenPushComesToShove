using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : MovementController
{
    public void MoveTo(Vector2 target)
    {
        move.enabled = true;
        move.target = target;
        startMovingEvent.Invoke();
    }

    public void FixedUpdate()
    {
        if(Vector2.Distance(transform.position, move.target) < destinationTolerance)
        {
            if(move.enabled)
            {
                endMovingEvent.Invoke();
            }
            move.enabled = false;
        }
    }
}
