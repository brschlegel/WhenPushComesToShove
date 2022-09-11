using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireShove : HitHandler
{
    [SerializeField]private float burnTime = 5;
    [SerializeField] private float timeBetweenBurns = 1;
    [SerializeField] private float burnDamage = 1;
    private Coroutine burnRoutine;
    private float timer = 0;

    public override void ReceiveHit(HitEvent e)
    {
        if (burnRoutine != null)
        {
            StopCoroutine(burnRoutine);
        }

        burnRoutine = StartCoroutine(BurnOverTime(e));
    }

    public IEnumerator BurnOverTime(HitEvent e)
    {
        Health health = e.hitbox.transform.parent.GetComponentInChildren<Health>();
        timer = 0;

        while (timer < burnTime)
        {
            yield return new WaitForSeconds(timeBetweenBurns);
            health.TakeDamage(burnDamage);
            timer += timeBetweenBurns;
        }
    }
}
