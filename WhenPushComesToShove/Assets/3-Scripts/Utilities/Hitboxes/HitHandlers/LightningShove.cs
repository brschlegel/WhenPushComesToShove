using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningShove : HitHandler
{
    [SerializeField] private float shockTime = 5;
    [SerializeField] private float timeBetweenShocks = 1;
    [SerializeField] private float timeToBeShocked = .1f;
    [SerializeField] private string shockID = "Shock";
    private float timer = 0;

    public override void ReceiveHit(HitEvent e)
    {
        //Check if enemy if already burning
        EnemyStatusAilments enemyAilments = e.hurtbox.GetComponent<EnemyStatusAilments>();

        if (enemyAilments.statusAilments.ContainsKey(shockID))
        {
            StopCoroutine(enemyAilments.statusAilments[shockID]);
            e.hurtbox.transform.parent.GetComponent<VelocitySetter>().UnHalt();
            enemyAilments.statusAilments[shockID] = StartCoroutine(ShockOverTime(e, enemyAilments));
        }
        else
        {

            enemyAilments.statusAilments.Add(shockID, StartCoroutine(ShockOverTime(e, enemyAilments)));
        }
    }
    /// <summary>
    /// Causes the afflicted to be stuned every so often over a period of time
    /// </summary>
    /// <param name="e"></param>
    /// <param name="ailmentRef"></param>
    /// <returns></returns>
    public IEnumerator ShockOverTime(HitEvent e, EnemyStatusAilments ailmentRef)
    {
        timer = 0;

        GameObject enemy = e.hurtbox.transform.parent.gameObject;

        while (timer < shockTime)
        {
            enemy.GetComponent<VelocitySetter>().Halt();
            Debug.Log("Shock");
            yield return new WaitForSeconds(timeToBeShocked);

            Debug.Log("Unstun");
            enemy.GetComponent<VelocitySetter>().UnHalt();

            yield return new WaitForSeconds(timeBetweenShocks);

            timer += timeBetweenShocks + timeToBeShocked;
        }

        ailmentRef.statusAilments.Remove(shockID);
    }
}

