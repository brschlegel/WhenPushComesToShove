using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleanSelfUpSeconds : MonoBehaviour
{
    [SerializeField]
    private float seconds;

    private void OnEnable()
    {
        CoroutineManager.StartGlobalCoroutine(Destroy());
    }
    private IEnumerator Destroy()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }
}
