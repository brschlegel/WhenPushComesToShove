using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisBrain : StateBrain
{
    AegisSetup setupState;
    AegisIdle idleState;
    AegisRun runState;
    AegisHit hitState;
    AegisAttack attackState;
    AegisBlock blockState;
    PlayAnimState deathState;
    AegisStun stunState;

    [SerializeField]
    AegisWall wall;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Chase chase;
    [SerializeField]
    private ProjectileMode pMode;
    [SerializeField]
    private EventOnHit hitEvent;
    [SerializeField]
    private GameObject hitboxObject;
    [SerializeField]
    private WallFilterHitHandler wallFilter;
    [SerializeField]
    private GameObject rootObject;
    [SerializeField]
    private TagFilterHitHandler tagFilter;
    // Start is called before the first frame update
    void Start() 
    {
        if(setupState == null)
            Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameState.players.Count > 0)
        {
            if (target == null || target.GetComponentInChildren<PlayerHealth>().dead)
            {
                target = GameState.GetNearestPlayer(transform);
            }
        }
        wall.target = target;
    }

    public void Init()  
    {
        setupState = GetComponent<AegisSetup>();
        idleState = GetComponent<AegisIdle>();
        runState = GetComponent<AegisRun>();
        hitState = GetComponent<AegisHit>();
        attackState = GetComponent<AegisAttack>();
        blockState = GetComponent<AegisBlock>();
        deathState = GetComponent<PlayAnimState>();
        stunState = GetComponent<AegisStun>();

        setupState.anim = anim;
        setupState.onStateExit += OutSetup;

        idleState.anim = anim;
        idleState.onStateExit += OutIdle;

        runState.anim = anim;
        runState.chase = chase;
        runState.onStateExit += OutRun;

        hitState.anim = anim;
        hitEvent.onHit += OnHit;
        hitState.pMode = pMode;
        hitState.animName = "Base Layer.Enemy Hit";
        hitState.wall = wall;
        hitState.movement = chase;
        hitState.onStateExit += OutHit;

        stunState.anim = anim;
        stunState.animName = "Base Layer.Enemy Hit";
        stunState.movement = chase;
        stunState.wall = wall;
        stunState.onStateExit += OutStun;
        tagFilter.onHitWithTag += OnStun;


        attackState.anim = anim;
        attackState.wall = wall;
        attackState.hitboxObject = hitboxObject;
        attackState.chase = chase;
        attackState.onStateExit += OutAttack;

        blockState.anim = anim;
        blockState.wall = wall;
        wallFilter.onBlock += OnBlock;
        blockState.onStateExit += OutBlock;

        deathState.anim = anim;
        deathState.animName = "Base Layer.Enemy Death";
        deathState.onStateExit += OutDeath;

        currentState = setupState;
        setupState.enabled = true;
    }

    public void OutSetup(bool success)
    {
        ChangeState(idleState);
    }

    public void OutIdle(bool success)
    {
        runState.target = target;
        ChangeState(runState);

    }

    private void OutRun(bool success)
    {
        if(success)
        {
           ChangeState(attackState);
        }
    }

    private void OutHit(bool success)
    {
        if(success)
        {
         
            runState.target = target;
            ChangeState(runState);
        }
    }

    private void OutBlock(bool success)
    {
        if(success)
        {
            runState.target = target;
            ChangeState(runState);
        }

    }

    private void OutAttack(bool success)
    {
        if(success)
        {
            runState.target = target;
            ChangeState(runState);
        }
    }

    private void OutDeath(bool success)
    {
        if(success)
        {
            Destroy(rootObject);
        }
    }

    private void OutStun(bool success)
    {
        if(success)
        {
            ChangeState(runState);
        }
    }

    public void OnHit(HitEvent e)
    {

        if (currentState != deathState)
        {
            if (e.hitbox.tag == "Shove")
            {
                target = e.hitbox.owner.transform;
            }
            ChangeState(hitState);
        }
    }

    private void OnBlock()
    {
        ChangeState(blockState);
    }

    public void OnDeath()
    {
        ChangeState(deathState);
        chase.LockMovement();
    }

    public void OnStun(string tag)
    {
        ChangeState(stunState);
    }
}
