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
            e.hurtbox.transform.parent.GetComponentInChildren<MovementController>().UnlockMovement();
            enemyAilments.statusAilments[shockID] = StartCoroutine(ShockOverTime(e, enemyAilments));
        }
        else
        {

            enemyAilments.statusAilments.Add(shockID, StartCoroutine(ShockOverTime(e, enemyAilments)));
        }
    }

    public IEnumerator ShockOverTime(HitEvent e, EnemyStatusAilments ailmentRef)
    {
        timer = 0;

        while (timer < shockTime)
        {
            StartCoroutine(StunEnemy(e.hurtbox.transform.parent.gameObject));
            Debug.Log("Shock");
            yield return new WaitForSeconds(timeToBeShocked);

            Debug.Log("Unstun");

            yield return new WaitForSeconds(timeBetweenShocks);

            timer += timeBetweenShocks + timeToBeShocked;
        }

        ailmentRef.statusAilments.Remove(shockID);
    }

    private IEnumerator StunEnemy(GameObject enemy)
    {
        Debug.Log(enemy);
        EnemyState[] states = enemy.GetComponentsInChildren<EnemyState>();

        EnemyState activeState = null;

        for (int i = 0; i < states.Length; i++)
        {
            if (states[i].enabled)
            {
                activeState = states[i];
                break;
            }
        }

        enemy.GetComponentInChildren<MovementController>().LockMovement();
        activeState.enabled = false; 

        yield return new WaitForSeconds(timeToBeShocked);

        enemy.GetComponentInChildren<MovementController>().UnlockMovement();
        activeState.enabled = true;
    }
}

