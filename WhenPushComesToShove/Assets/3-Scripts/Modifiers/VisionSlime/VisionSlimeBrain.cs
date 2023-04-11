using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Flip on run
//Do attack state
//Actually set up modifier object
public class VisionSlimeBrain : StateBrain
{
    VisionSlimeIdleState idleState;
    VisionSlimeRunState runState;
    VisionSlimeDecideState decideState;
    VisionSlimeAttackState attackState;
    PlayAnimState landState;
    
    [Header("Next Action Weights")]
    [SerializeField]
    private float idleThresh;
    [SerializeField]
    private float runThresh;
    [SerializeField]
    private float attackThresh;

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private SpriteRenderer sprite;
    private void Start()
    {
        if(idleState == null)
        {
            Init();
        }
    }

    private void Init()
    {
        idleState = GetComponent<VisionSlimeIdleState>();
        runState = GetComponent<VisionSlimeRunState>();
        decideState = GetComponent<VisionSlimeDecideState>();
        attackState = GetComponent<VisionSlimeAttackState>();
        landState = GetComponent<PlayAnimState>();

        idleState.anim = anim;
        idleState.onStateExit += OutState;

        runState.anim = anim;
        runState.sprite = sprite;
        runState.onStateExit += OutState;

        decideState.anim = anim;
        decideState.decideTime = 1f;
        decideState.onStateExit += OutDecide;

        attackState.anim = anim;
        attackState.sprite = sprite;
        attackState.onStateExit += OutAttack;

        landState.anim = anim;
        landState.animName = "Slime_Land";
        landState.onStateExit += OutState;

        currentState = idleState;
        currentState.enabled = true;
    }

    private void OutState(bool success)
    {
        ChangeState(decideState);
    }

    private void OutDecide(bool success)
    {
        ChangeState(PickNextState());
    }

    private void OutAttack(bool success)
    {
        ChangeState(landState);
    }


    //Yeah yeah I know this isn't really great, but this isnt a huge feature dont want to spend too much time on it
    private State PickNextState()
    {
        float rand = Random.value;
        if(rand <= idleThresh )
        {
            idleState.time = Random.value * 10 + 2;
            return idleState;
        }
        else if(rand <= runThresh)
        {
            runState.target = PickRunLocation();
            return runState;
        }
        else if(rand <= attackThresh)
        {
            attackState.target = (Vector2)transform.position + Random.insideUnitCircle.normalized * 4;
            return attackState;
        }
        return idleState;
    }

    private Vector2 PickRunLocation()
    {
        Vector2 location =  new Vector2(Random.value * 20 - 10, Random.value * 9 - 4.5f);
        int failsafe = 0;
        //Just ensure the location is a certain distance away
        //Make sure this doesn't try too many times for whatever reason
        while(Vector2.Distance((Vector2)transform.position, location) <= .5f && failsafe < 10)
        {
            location = new Vector2(Random.value * 22 - 11, Random.value * 10 - 5);
            failsafe++;
        }
        return location;
    }
}
