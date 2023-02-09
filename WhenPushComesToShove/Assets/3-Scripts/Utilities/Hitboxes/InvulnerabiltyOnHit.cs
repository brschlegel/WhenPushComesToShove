using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvulnerabiltyOnHit : HitHandler
{
    [SerializeField] private float timeForInvulnerability = .05f;
    [SerializeField] private GameObject hurtboxRef;
    [SerializeField] private Health health;

    public override void ReceiveHit(HitEvent e)
    {
        if (!health.dead)
        {
            StartCoroutine(Cooldown());
        }
    }

    private IEnumerator Cooldown()
    {
        hurtboxRef.SetActive(false);
        yield return new WaitForSeconds(timeForInvulnerability);
        hurtboxRef.SetActive(true);
    }
}
