using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthShove : HitHandler
{
    [SerializeField] private float burnTime = 5;
    [SerializeField] private float timeBetweenBurns = 1;
    [SerializeField] private float burnDamage = 1;
    [SerializeField] private string burnID = "Burn";
    private float timer = 0;

    public override void ReceiveHit(HitEvent e)
    {
        //Check if enemy if already burning
        EnemyStatusAilments enemyAilments = e.hurtbox.GetComponent<EnemyStatusAilments>();

        if (enemyAilments.statusAilments.ContainsKey(burnID))
        {
            StopCoroutine(enemyAilments.statusAilments[burnID]);
            enemyAilments.statusAilments[burnID] = StartCoroutine(BurnOverTime(e, enemyAilments));
        }
        else
        {

            enemyAilments.statusAilments.Add(burnID, StartCoroutine(BurnOverTime(e, enemyAilments)));
        }
    }

    public IEnumerator BurnOverTime(HitEvent e, EnemyStatusAilments ailmentRef)
    {
        Health health = e.hurtbox.transform.parent.GetComponentInChildren<Health>();
        timer = 0;

        while (timer < burnTime)
        {
            yield return new WaitForSeconds(timeBetweenBurns);
            health.TakeDamage(burnDamage);
            Debug.Log("Burn");
            timer += timeBetweenBurns;
        }

        ailmentRef.statusAilments.Remove(burnID);
    }
}