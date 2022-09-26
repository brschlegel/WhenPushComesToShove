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
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        if(setupState == null)
            Init();
    }

    // Update is called once per frame
    void Update()
    {
        
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

        runState.anim = anim;

        hitState.anim = anim;

        currentState = setupState;
        setupState.enabled = true;
    }

    public void OutSetup(bool success)
    {
        ChangeState(idleState);
    }
}
