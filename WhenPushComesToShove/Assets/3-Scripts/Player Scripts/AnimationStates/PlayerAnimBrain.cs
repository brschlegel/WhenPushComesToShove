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
    PlayerLightShoveState lightState;
    PlayerChargeState chargeState;
    PlayerHeavyShoveState heavyState;
    //#endregion

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private VelocitySetter vs;
    [SerializeField]
    private Hitstun hitstun;
    [SerializeField]
    private PlayerDashScript dashScript;
    [SerializeField]
    private PlayerInputHandler playerInputHandler;

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
        lightState = GetComponent<PlayerLightShoveState>();
        chargeState = GetComponent<PlayerChargeState>();
        heavyState = GetComponent<PlayerHeavyShoveState>();

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

        lightState.anim = anim;
        playerInputHandler.GetComponent<PlayerLightShoveScript>().onLightShove += OnLightShove;
        playerInputHandler.onLightShoveComplete += OutShove;

        chargeState.anim = anim;

        heavyState.anim = anim;
        playerInputHandler.GetComponent<PlayerHeavyShoveScript>().onHeavyShove += OnHeavyShove;
        playerInputHandler.onHeavyShoveComplete += OutShove;

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

    private void OutShove()
    {
        ChangeState(idleState);
    }

    public void OnHit()
    {
        ChangeState(hitState);
    }

    public void OnDash(Vector3 dir)
    {
        ChangeState(dashState);
    }

    public void OnLightShove()
    {
        ChangeState(lightState);
    }

    public void OnHeavyShove()
    {   
        ChangeState(heavyState);
    }

    /// <summary>
    /// How does velocity magnitude compare to zero
    /// </summary>
    /// <param name="id">Source id</param>
    /// <param name="greaterThanZero">Checking if greater than zero</param>
    /// <returns></returns>
    private bool CheckVelocityMagnitude(string id, bool greaterThanZero)
    {
        if(vs.QuerySource(id, out Vector2 vel))
        {
            return (vel.magnitude >= .01f) == greaterThanZero;
        }
        return !greaterThanZero;
    }
}
