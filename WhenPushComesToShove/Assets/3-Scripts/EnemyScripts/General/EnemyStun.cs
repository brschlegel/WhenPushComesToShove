using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStun : State
{

    [HideInInspector]
    public MovementController movement;
    [HideInInspector]
    public string animName;

    [SerializeField]
    private float stunTime;
    private IEnumerator enumerator;

    protected virtual void OnEnable()
    {
        enumerator = CoroutineManager.StartGlobalCoroutine(Stun());
        anim.Play(animName, 0);
        movement.LockMovement();
    }

    private IEnumerator Stun()
    {
        yield return new WaitForSeconds(stunTime); 
        this.enabled = false;
        InvokeOnStateExit(true);
    }

    protected virtual void OnDisable()
    {
        CoroutineManager.StopGlobalCoroutine(enumerator);
        movement.UnlockMovement();
    }

}
