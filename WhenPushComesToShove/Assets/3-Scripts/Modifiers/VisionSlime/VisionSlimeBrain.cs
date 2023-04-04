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
    
    [Header("Next Action Weights")]
    [SerializeField]
    private float idleThresh;
    [SerializeField]
    private float runThresh;
    [SerializeField]
    private float attackThresh;

    [SerializeField]
    private Animator anim;
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

        idleState.anim = anim;
        idleState.onStateExit += OutState;

        runState.anim = anim;
        idleState.onStateExit += OutState;

        decideState.anim = anim;
        decideState.decideTime = 1f;
        decideState.onStateExit += OutDecide;

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
            return idleState;
        }
        return idleState;
    }

    private Vector2 PickRunLocation()
    {
        Vector2 location =  new Vector2(Random.value * 22 - 11, Random.value * 10 - 5);
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
