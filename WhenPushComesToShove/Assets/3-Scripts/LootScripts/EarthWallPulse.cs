using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class EarthWallPulse : MonoBehaviour
{
    [SerializeField] private float timePerPulse = 2;
    [SerializeField] private float endScale = 2f;
    private Vector3 originalScale;

    private void OnEnable()
    {
        originalScale = transform.localScale;
        StartCoroutine(Pulse());
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
}
