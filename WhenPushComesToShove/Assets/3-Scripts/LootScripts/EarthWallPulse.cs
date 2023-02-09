using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class EarthWallPulse : HitHandler
{
    [SerializeField] private float timePerPulse = 2;
    [SerializeField] private float endScale = 2f;
    private Vector3 originalScale;

    [SerializeField] private float timeBeforeDestroy = 2;

    private void OnEnable()
    {
        originalScale = transform.localScale;
        StartCoroutine(Pulse());
        StartCoroutine(DestroyWallOverTime());
    }

    /// <summary>
    /// Expands the wall's hit box over time
    /// </summary>
    /// <returns></returns>
    private IEnumerator Pulse()
    {
        transform.DOScale(originalScale * endScale, timePerPulse);
        yield return new WaitForSeconds(timePerPulse);

        transform.DOKill();
        transform.localScale = originalScale;

        StartCoroutine(Pulse());
    }


    private IEnumerator DestroyWallOverTime()
    {
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(transform.parent.gameObject);
    }

    public override void ReceiveHit(HitEvent e)
    {
        
    }
}
