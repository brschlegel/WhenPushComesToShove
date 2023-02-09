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
    [HideInInspector]
    public MovementController movement;
    [SerializeField]
    private float additionalStunTime;

    protected virtual void OnEnable()
    {
        anim.Play(animName, 0);
        enumerator = CoroutineManager.StartGlobalCoroutine(Stun());
        movement.LockMovement();
    }

    protected IEnumerator Stun()
    {
        yield return new WaitUntil (()=>!pMode.enabled);
        yield return new WaitForSeconds(additionalStunTime);
        this.enabled = false;
        InvokeOnStateExit(true);
    }

    protected virtual void OnDisable()
    {
        CoroutineManager.StopGlobalCoroutine(enumerator);
        movement.UnlockMovement();
    }
}
