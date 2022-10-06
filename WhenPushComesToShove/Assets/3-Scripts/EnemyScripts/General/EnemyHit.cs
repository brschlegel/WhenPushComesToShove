using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : State
{
    private IEnumerator enumerator;

    [HideInInspector]
    public ProjectileMode pMode;
    [HideInInspector]
    public string animName;
    [SerializeField]
    private float additionalStunTime = 0;

    private void OnEnable()
    {
        anim.Play(animName, 0);
        enumerator = CoroutineManager.StartGlobalCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        yield return new WaitUntil (()=>!pMode.enabled);
        yield return new WaitForSeconds(additionalStunTime);
        this.enabled = false;
        InvokeOnStateExit(true);
    }

    private void OnDisable()
    {
        CoroutineManager.StopGlobalCoroutine(enumerator);
    }
}
