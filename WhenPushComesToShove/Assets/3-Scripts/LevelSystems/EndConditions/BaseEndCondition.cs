using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEndCondition : MonoBehaviour
{
    protected LevelProperties levelProp;
    [Tooltip("Time between the end condition being met and transitioning to the next room.")]
    [SerializeField] protected float delayBeforeEndCondition;
    protected WaitForSeconds delay;
    protected bool conditionMet = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        levelProp = gameObject.transform.parent.parent.GetComponent<LevelProperties>();
        delay = new WaitForSeconds(delayBeforeEndCondition);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!conditionMet)
            TestCondition();
    }

    
    protected virtual void TestCondition()
    {
        conditionMet = true;
        StartCoroutine(TransitionRooms());
    }

    protected IEnumerator TransitionRooms()
    {
        yield return delay;
        LevelManager.onNewRoom.Invoke();
    }
}
