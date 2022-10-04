using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisBrain : StateBrain
{
    AegisSetup setupState;
    AegisIdle idleState;
    AegisRun runState;
    EnemyHit hitState;
    AegisAttack attackState;

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
        hitState = GetComponent<EnemyHit>();
        attackState = GetComponent<AegisAttack>();

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
        hitState.onStateExit += OutHit;

        attackState.anim = anim;
 
        attackState.onStateExit += OutAttack;

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

    private void OutAttack(bool success)
    {
        if(success)
        {
            runState.target = target;
            ChangeState(runState);
        }
    }

    public void OnHit(GameObject instigator, GameObject receiver)
    {
        target = instigator.transform;
        ChangeState(hitState);
    }
}
