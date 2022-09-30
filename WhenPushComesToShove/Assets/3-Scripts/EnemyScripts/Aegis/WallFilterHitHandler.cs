using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallFilterHitHandler : HitHandler
{
 
    public override void ReceiveHit(HitEvent e)
    {
        Debug.Log("Blocked");
    }
}
