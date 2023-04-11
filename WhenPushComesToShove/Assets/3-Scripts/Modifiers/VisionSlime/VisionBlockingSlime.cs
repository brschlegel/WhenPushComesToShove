using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionBlockingSlime : MonoBehaviour
{
    [SerializeField]
    private VisionSlimeBrain brain;
    private IEnumerator MoveToSpot(Vector2 spot)
    {
        yield return new WaitUntil(() => (Vector2)transform.position == spot);
    }

    private IEnumerator ChillForABit(float time)
    {
        yield return new WaitForSeconds(time);
    }

    
}
