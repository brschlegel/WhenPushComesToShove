using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimBrain : StateBrain
{
    //#region States
    PlayerIdleState idleState;
    PlayerRunState runState;
    //#endregion

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private VelocitySetter vs;

    private void Start()
    {
        if(idleState == null)
        {
            Init();
        }
    }

    private void Update()
    {
        switch(currentState.id)
        {
            case "idle":
                if(vs.QuerySource("playerMovement", out Vector2 vel))
                {
                    if(vel.magnitude >= .01f)
                    {
                        currentState.enabled = false;
                        currentState = runState;
                        currentState.enabled = true;
                    }
                }
                break;
            case "run":
                if(vs.QuerySource("playerMovement", out Vector2 rVel))
                {
                    if(rVel.magnitude < .01f)
                    {
                        currentState.enabled = false;
                        currentState = idleState;
                        currentState.enabled = true;
                    }
                }
                break;
        }
    }

    private void Init()
    {
        idleState = GetComponent<PlayerIdleState>();
        runState = GetComponent<PlayerRunState>();

        idleState.anim = anim;
        idleState.onStateExit += OutIdle;

        runState.anim = anim;
        runState.onStateExit += OutRun;

        currentState = idleState;
        currentState.enabled = true;
    }

    private void OutIdle(bool success)
    {

    }

    private void OutRun(bool success)
    {

    }
}
