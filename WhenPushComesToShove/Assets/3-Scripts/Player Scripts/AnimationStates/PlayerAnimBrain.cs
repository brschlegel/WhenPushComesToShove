using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimBrain : StateBrain
{
    //#region States
    PlayerIdleState idleState;
    PlayerRunState runState;
    PlayerHitState hitState;
    PlayerDashState dashState;
    //#endregion

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private Hitstun hitstun;
    [SerializeField]
    private PlayerDashScript dashScript;

    private void Start()
    {
        if(idleState == null)
        {
            Init();
        }
    }

    private void Update()
    {
        
    }

    private void Init()
    {
        idleState = GetComponent<PlayerIdleState>();
        runState = GetComponent<PlayerRunState>();
        hitState = GetComponent<PlayerHitState>();
        dashState = GetComponent<PlayerDashState>();

        idleState.anim = anim;
        idleState.vs = vs;
        idleState.onStateExit += OutIdle;

        runState.anim = anim;
        runState.vs = vs;
        runState.onStateExit += OutRun;

        hitState.anim = anim;
        hitState.hitstun = hitstun;
        hitState.onStateExit += OutHit;

        dashState.anim = anim;
        dashState.vs = vs;
        dashScript.onDashStart += OnDash;
        dashState.onStateExit += OutDash;

        currentState = idleState;
        currentState.enabled = true;
    }

    private void OutIdle(bool success)
    {
        if(success)
        {
            currentState = runState;
            currentState.enabled = true;
        }
    }

    private void OutRun(bool success)
    {
        if(success)
        {
            currentState = idleState;
            currentState.enabled = true;
        }
    }

    private void OutHit(bool success)
    {
        if(success)
        {
            currentState = idleState;
            currentState.enabled = true;
        }
    }

    private void OutDash(bool success)
    {
        if(success)
        {
            currentState = idleState;
            currentState.enabled = true;
        }
    }

    public void OnHit()
    {
        currentState.enabled = false;
        currentState = hitState;
        currentState.enabled = true;
    }

    public void OnDash(Vector3 dir)
    {
        currentState.enabled = false;
        currentState = dashState;
        currentState.enabled = true;
    }

    private bool CheckVelocityMagnitude(string id, bool greaterThanZero)
    {
        if(vs.QuerySource(id, out Vector2 vel))
        {
            return (vel.magnitude >= .01f) == greaterThanZero;
        }
        return !greaterThanZero;
    }
}
