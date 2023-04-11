using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionSlimeDecideState : State
{
    [HideInInspector]
    public float decideTime;

    private void OnEnable()
    {
        anim.Play("Slime_Idle", 0);
        StartCoroutine(WaitToDecide());
    }

    private IEnumerator WaitToDecide()
    {
        yield return new WaitForSeconds(decideTime);
        enabled = false;
        InvokeOnStateExit(true);
    }
}
