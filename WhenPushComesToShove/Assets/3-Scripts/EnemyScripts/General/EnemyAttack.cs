using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void AttackEnded(Transform target);

public abstract class EnemyAttack : MonoBehaviour
{
    protected Coroutine coroutine;
    public abstract void Cancel();
    public event AttackEnded onAttackEnd;

    protected void InvokeAttackEnd(Transform target)
    {
        onAttackEnd.Invoke(target);
    }

    public virtual void Attack(Transform target)
    {
       
    }

    protected abstract IEnumerator AttackCoroutine(Transform target);
}
