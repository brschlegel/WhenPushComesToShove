using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceShove : HitHandler
{
    [SerializeField] private float slowTime = 5;
    [SerializeField] private float percentSpeedDecrease = .5f;
    [SerializeField] private string slowID = "SlowID";
    private Coroutine slowRoutine;

    public override void ReceiveHit(HitEvent e)
    {
        //Check if enemy if already slowed
        EnemyStatusAilments enemyAilments = e.hurtbox.GetComponent<EnemyStatusAilments>();

        if (enemyAilments.statusAilments.ContainsKey(slowID))
        {
            StopCoroutine(enemyAilments.statusAilments[slowID]);
            e.hurtbox.transform.parent.GetComponentInChildren<MovementController>().Speed /= percentSpeedDecrease;
            enemyAilments.statusAilments[slowID] = StartCoroutine(Slow(e, enemyAilments));
        }
        else
        {
            enemyAilments.statusAilments.Add(slowID, StartCoroutine(Slow(e, enemyAilments)));
        }
    }

    public IEnumerator Slow(HitEvent e, EnemyStatusAilments ailmentRef)
    {
        MovementController move = e.hurtbox.transform.parent.GetComponentInChildren<MovementController>();
        move.Speed *= percentSpeedDecrease;
        Debug.Log("Slow");

        yield return new WaitForSeconds(slowTime);

        move.Speed /= percentSpeedDecrease;
    }
}
