using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimBrain : StateBrain
{
    //#region States
    PlayerIdleState idleState;
    PlayerRunState runState;
    PlayerHitState hitState;
    PlayAnimState dashState;
    PlayAnimState lightState;
    PlayerChargeState chargeState;
    PlayerHeavyShoveState heavyState;
    PlayAnimState deathState;
    PlayAnimState leftEmoteState;
    //#endregion

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private ProjectileMode pMode;
    [SerializeField]
    private PlayerDashScript dashScript;
    [SerializeField]
    private PlayerInputHandler playerInputHandler;
    [SerializeField]
    private PlayerMovementScript mover;
    [SerializeField]
    private ParticleSystem shoveExclamation;
    [SerializeField]
    private EventOnHit hitEvent;

    private PlayerHeavyShoveScript heavyScript;
    private PlayerLightShoveScript lightScript;

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
       // lightState = GetComponent<PlayerLightShoveState>();
        chargeState = GetComponent<PlayerChargeState>();
        heavyState = GetComponent<PlayerHeavyShoveState>();
        heavyScript = playerInputHandler.GetComponent<PlayerHeavyShoveScript>();
        lightScript = playerInputHandler.GetComponent<PlayerLightShoveScript>();

        PlayAnimState[] animStates = GetComponents<PlayAnimState>();
        foreach(PlayAnimState state in animStates)
        {
            switch(state.id)
            {
                case "dash":
                    dashState = state;
                    dashState.animName = "Base Layer.AN_Player_Run";
                    break;
                case "death":
                    deathState = state;
                    deathState.animName = "Base Layer.AN_Player_Dead";
                    break;
                case "light":
                    lightState = state;
                    lightState.animName = "Base Layer.AN_Player_LightShove";
                    break;
                case "leftEmote":
                    leftEmoteState = state;
                    leftEmoteState.animName = "Base Layer.AN_Player_ChestHuff";
                    break;
                default:
                    Debug.LogError("UNKNOWN ANIM STATE ID: " + state.id);
                    break;
            }
        }

        idleState.anim = anim;
        idleState.mover = mover;
        idleState.onStateExit += OutIdle;

        runState.anim = anim;
        runState.mover = mover;
        runState.onStateExit += OutRun;

        hitState.anim = anim;
        hitState.pMode = pMode;
        hitEvent.onHit += OnHit;
        //hitEvent.onHit += playerInputHandler.GetComponent<PlayerHeavyShoveScript>().InterruptChargeOnHit;
        hitState.onStateExit += OutHit;

        dashState.anim = anim;
        dashScript.onDashStart += OnDash;
        dashState.onStateExit += OutDash;

        lightState.anim = anim;
        lightScript.onLightShove += OnLightShove;
        //playerInputHandler.onLightShoveComplete += OutShove;
        lightState.onStateExit += OutLight;

        chargeState.anim = anim;
        heavyScript.onHeavyCharge += OnHeavyCharge;
        heavyScript.onHeavyFail += OnHeavyFail;

        heavyState.anim = anim;
        heavyState.heavyShoveScript = heavyScript;
        heavyScript.onHeavyShove += OnHeavyShove;
        playerInputHandler.onHeavyShoveComplete += OutShove;

        deathState.anim = anim;
        deathState.onStateExit += OutDeath;

        leftEmoteState.anim = anim;
        playerInputHandler.onLeftEmote += OnLeftEmote;
        playerInputHandler.onLeftEmoteEnd += OutLeftEmote;

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
            if(heavyScript.heavyShoveIsCharging)
            {
                Debug.Log("ch");
                ChangeState(chargeState);
            }
            else
            {
            currentState = idleState;
            currentState.enabled = true;
            }
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

    private void OutDeath(bool success)
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

    private void OutLight(bool success)
    {
        ChangeState(idleState);
    }

    private void OutLeftEmote()
    {
        ChangeState(idleState);
    }


    public void OnHit(HitEvent e)
    {
        if (currentState != deathState)
        {
            ChangeState(hitState);
        }
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

    public void OnHeavyCharge()
    {
        ChangeState(chargeState);
    }

    public void OnDeath()
    {
        ChangeState(deathState);
    }
    
    public void OnHeavyFail()
    {
        ChangeState(idleState);
    }

    public void OnLeftEmote()
    {
        ChangeState(leftEmoteState);
    }

   
}
