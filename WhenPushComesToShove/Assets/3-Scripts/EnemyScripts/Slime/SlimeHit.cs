using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHit : EnemyState
{
    private IEnumerator enumerator;

    [HideInInspector]
    public EnemyHitstun hitstun;
    [SerializeField]
    public float additionalStunTime;
    private void OnEnable()
    {
        anim.Play("Base.Slime_Hit", 0);
        enumerator = CoroutineManager.StartGlobalCoroutine(Stun());
    }

    private IEnumerator Stun()
    {
        yield return new WaitUntil (()=>!hitstun.inHitstun);
        yield return new WaitForSeconds(additionalStunTime);
        this.enabled = false;
        InvokeOnStateExit(true);
    }

    private void OnDisable()
    {
        CoroutineManager.StopGlobalCoroutine(enumerator);
    }
}
