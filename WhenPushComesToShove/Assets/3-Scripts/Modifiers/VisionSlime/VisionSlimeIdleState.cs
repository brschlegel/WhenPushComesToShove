using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSlimeIdleState : State
{
    [HideInInspector]
    public float time;

    private void OnEnable()
    {  
        anim.Play("Slime_Idle", 0);
        StartCoroutine(Wait());
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(time);
        enabled = false;
        InvokeOnStateExit(true);
    }
}
