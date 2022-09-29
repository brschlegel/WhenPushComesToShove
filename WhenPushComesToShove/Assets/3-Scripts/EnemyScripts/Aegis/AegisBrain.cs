using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisBrain : StateBrain
{
    AegisSetup setupState;
    AegisIdle idleState;
    AegisRun runState;
    AegisHit hitState;

    [SerializeField]
    AegisWall wall;
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Chase chase;
    // Start is called before the first frame update
    void Start() 
    {
        if(setupState == null)
            Init();
    }

    // Update is called once per frame
    void Update()
    {
        wall.target = target;
    }

    public void Init()  
    {
        setupState = GetComponent<AegisSetup>();
        idleState = GetComponent<AegisIdle>();
        runState = GetComponent<AegisRun>();
        hitState = GetComponent<AegisHit>();

        setupState.anim = anim;
        setupState.onStateExit += OutSetup;

        idleState.anim = anim;
        idleState.onStateExit += OutIdle;

        runState.anim = anim;
        runState.chase = chase;
        runState.onStateExit += OutRun;

        hitState.anim = anim;

        currentState = setupState;
        setupState.enabled = true;
    }

    public void OutSetup(bool success)
    {
        ChangeState(idleState);
    }

    public void OutIdle(bool success)
    {
        ChangeState(runState);
    }

    private void OutRun(bool success)
    {
        if(success)
        {
            target = runState.target;
            
        }
    }
}
