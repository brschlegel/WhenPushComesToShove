using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : HitHandler
{
    public override void ReceiveHit(HitEvent e)
    {
        Destroy(gameObject);
    }
}
