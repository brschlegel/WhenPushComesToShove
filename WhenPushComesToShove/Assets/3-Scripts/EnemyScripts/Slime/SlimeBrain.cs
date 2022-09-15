using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBrain : StateBrain
{
    //#region States
    SlimeIdle idleState;
    SlimeRun runState;
    SlimeJump jumpState;
    SlimeLand landState;
    SlimeHit hitState;
    //#endregion

    [SerializeField]
    private Chase chase;
    [SerializeField]
    private EnemyHitstun hitstun;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject hitboxObject;



    private void Start()
    {
        if(runState == null)
        {
            Init();
        }
    }

    private void Update()
    {
        if(runState.enabled)
            target = runState.target;
    }

    private void Init()
    {
        idleState = GetComponent<SlimeIdle>();
        runState = GetComponent<SlimeRun>();
        jumpState = GetComponent<SlimeJump>();
        landState = GetComponent<SlimeLand>();
        hitState = GetComponent<SlimeHit>();

        idleState.onStateExit += OutIdle;
        runState.chase = chase;
        runState.anim = anim;
        runState.onStateExit += OutRun;

        jumpState.chase = chase;
        jumpState.anim = anim;
        jumpState.onStateExit += OutJump;

        landState.chase = chase;
        landState.anim = anim;
        landState.hitboxObject = hitboxObject;
        landState.onStateExit += OutLand;

        hitState.anim = anim;
        hitState.hitstun = hitstun;
        hitState.onStateExit += OutHit;
        
    }

    private void OutLand(bool success)
    {
        if(success)
        {
            currentState = runState;
            runState.enabled = true;
        }
    }

    private void OutJump(bool success)
    {
        if(success)
        {
            currentState = landState;
            landState.enabled = true;
        }
    }

    private void OutRun(bool success)
    {
        if(success)
        {
            currentState = jumpState;
            jumpState.target = target;
            jumpState.enabled = true;
        }
    }

    private void OutIdle(bool success)
    {
        if(success)
        {
            currentState = runState;
            runState.enabled = true;
        }
    }

    private void OutHit(bool success)
    {
        if(success)
        {
            currentState = runState;
            runState.enabled = true;
        }
    }

    /// <summary>
    /// When slime gets hit, change to hit state
    /// </summary>
    public void OnHit()
    {
        currentState.enabled = false;
        currentState = hitState;
        hitState.enabled = true;
    }



  
}
