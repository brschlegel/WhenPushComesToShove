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
    EnemyHit hitState;
    PlayAnimState deathState;
    //#endregion


    [SerializeField]
    private Chase chase;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private GameObject hitboxObject;
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private ProjectileMode pMode;
    [SerializeField]
    private EventOnHit hitEvent;


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
        {
            target = runState.target;
        }
        else if(target != null)
        {
            if (target.GetComponentInChildren<Health>().dead)
            {
                target = null;
                chase.SetTarget(null);
            }   
        }
    }

    private void Init()
    {
        idleState = GetComponent<SlimeIdle>();
        runState = GetComponent<SlimeRun>();
        jumpState = GetComponent<SlimeJump>();
        landState = GetComponent<SlimeLand>();
        hitState = GetComponent<EnemyHit>();
        deathState = GetComponent<PlayAnimState>();

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

        deathState.anim = anim;
        deathState.onStateExit += OutDeath;
        deathState.animName = "Base.SpikedSlime_Death";

        hitState.anim = anim;
        hitState.animName = "Base.Slime_Hit";
        hitState.pMode = pMode;
        hitEvent.onHit += OnHit; 
        hitState.onStateExit += OutHit;
        

        currentState = idleState;
        currentState.enabled = true;
        
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

    private void OutDeath(bool success)
    {
        if(success)
        {
            Destroy(rootObject);
        }
    }

    /// <summary>
    /// When slime gets hit, change to hit state
    /// </summary>
    public void OnHit(HitEvent e)
    {
        if (currentState != deathState)
        {
            currentState.enabled = false;
            currentState = hitState;
            hitState.enabled = true;
        }
    }

    public void OnDeath()
    {
        currentState.enabled = false;
        currentState = deathState;
        deathState.enabled = true;
        chase.LockMovement();
    }



  
}
